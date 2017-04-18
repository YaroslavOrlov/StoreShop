var storeApp = angular.module('storeApp');
storeApp.controller('productsController', function ($scope, $http) {

    $scope.products = [];
    $http({ method: 'GET', url: 'Products/GetAll' }).then(function (response) {
        $scope.products = response.data;
    });

    $scope.cart = [];
    $scope.summaryPrice = 0;
    $scope.addToCart = function (product) {
        $scope.cart.push({
            Id: product.Id,
            Name: product.Name,
            Price: product.Price,
            Count: product.Count,
            CategoriesId: product.CategoriesId
        });
        $scope.summaryPrice += (product.Price * product.Count * $scope.currencyRate);
    };

    $scope.removeFromCart = function (product) {
        $scope.summaryPrice -= (product.Price * product.Count * $scope.currencyRate);
        $scope.cart.splice($scope.cart.indexOf(product), 1);
    };

    $scope.currencyRate = 1;
    $('#CurrencyId').on('change', function (e) {
        $http({
            method: 'GET',
            url: 'Currencies/GetRate',
            params: { 'id': $('#CurrencyId option:selected').val() }
        }).then(function successCallback(response) {
            $scope.summaryPrice /= $scope.currencyRate;
            $scope.currencyRate = response.data.Rate;
            $scope.summaryPrice *= $scope.currencyRate;
        });
    });

    $scope.isPay = false;
    $scope.discountedCart = [];
    $scope.makeOrder = function (discountCode) {
        $scope.cartPrice = $scope.summaryPrice;
        $http({
            method: 'POST',
            url: 'Cupons/ApplyCupon',
            data: { "discountCode": discountCode, "cart": $scope.cart }
        }).then(function(response) {
            $scope.discountedCart = response.data;
            $scope.summaryPrice = $scope.discountedCart.sum("Price") * $scope.currencyRate;
        });

        $scope.isPay = true;
    };

    $scope.changeOrder = function () {
        $scope.isPay = false;
        $scope.summaryPrice = $scope.cartPrice;
    };

    $scope.payForPurchase = function () {
        $http({
            method: 'POST',
            url: 'Purchases/Create',
            data: { "purchase": {
                discountedCart : $scope.discountedCart,
                summaryPrice : $scope.summaryPrice,
                currencyId : $('#CurrencyId option:selected').val()
            }}
        }).then(function successCallback(response) {
            $scope.cart = [];
            $scope.discountedCart = [];
            $scope.summaryPrice = 0;
            $scope.isPay = false;
            $(".alert.alert-success").removeClass("hidden");
            setTimeout(function () {
                $(".alert.alert-success").addClass("hidden");
            }, 3000);
        },function errorCallback(response){
            $(".alert.alert-danger").removeClass("hidden");
            setTimeout(function () {
                $(".alert.alert-danger").addClass("hidden");
            }, 3000);
        });
    };

    Array.prototype.sum = function (prop) {
        var total = 0
        for (var i = 0, _len = this.length; i < _len; i++) {
            total += this[i][prop]
        }
        return total
    }

});