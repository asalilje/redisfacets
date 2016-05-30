"use strict";

Object.defineProperty(exports, "__esModule", {
    value: true
});

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

var _Ajax = require("./Ajax.js");

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var FilterHotels = function () {
    function FilterHotels() {
        _classCallCheck(this, FilterHotels);

        this.filterSection = document.querySelector(".filter-section");
        this.hotelList = document.querySelector(".hotel-list");
        this.hotelListResult = document.querySelector(".hotel-list__result");
        this.sortBy = "";
        this.sortOrder = "";
        this.setupSortButtons();
        this.setupFilterButtons();
        this.setupPriceRange();
    }

    _createClass(FilterHotels, [{
        key: "setupFilterButtons",
        value: function setupFilterButtons() {
            var _this = this;

            var filterButtons = Array.prototype.slice.call(this.filterSection.querySelectorAll(".filter-button"));
            filterButtons.forEach(function (btn) {
                return btn.addEventListener("click", function (e) {
                    return _this.toggleFilter(e);
                });
            });
        }
    }, {
        key: "setupPriceRange",
        value: function setupPriceRange() {
            var _this2 = this;

            this.priceMin = this.filterSection.querySelector("input[name='priceMin']");
            this.priceMax = this.filterSection.querySelector("input[name='priceMax']");
            this.priceMin.addEventListener("keyup", function () {
                return _this2.loadHotels();
            });
            this.priceMax.addEventListener("keyup", function () {
                return _this2.loadHotels();
            });
        }
    }, {
        key: "setupSortButtons",
        value: function setupSortButtons() {
            var _this3 = this;

            this.sortButtons = Array.prototype.slice.call(this.hotelList.querySelectorAll(".sort-button"));
            this.sortButtons.forEach(function (btn) {
                return btn.addEventListener("click", function (e) {
                    return _this3.sortList(e);
                });
            });
        }
    }, {
        key: "toggleFilter",
        value: function toggleFilter(e) {
            if (e.target.getAttribute("data-filter-active") === "on") {
                e.target.setAttribute("data-filter-active", "off");
                e.target.classList.remove("filter-button--active");
            } else {
                e.target.setAttribute("data-filter-active", "on");
                e.target.classList.add("filter-button--active");
            }
            this.loadHotels();
        }
    }, {
        key: "sortList",
        value: function sortList(e) {
            this.sortBy = e.target.getAttribute("data-sort-by");
            var sortOrder = e.target.getAttribute("data-sort-order");
            this.sortButtons.forEach(function (x) {
                return x.setAttribute("data-sort-order", "");
            });

            if (sortOrder === "asc") {
                this.sortOrder = "desc";
                e.target.setAttribute("data-sort-order", "desc");
            } else {
                e.target.setAttribute("data-sort-order", "asc");
                this.sortOrder = "asc";
            }
            this.loadHotels();
        }
    }, {
        key: "getActiveFilters",
        value: function getActiveFilters() {
            var activeFilters = Array.prototype.slice.call(this.filterSection.querySelectorAll("[data-filter-active='on']"));
            var filterKeys = activeFilters.map(function (x) {
                return x.getAttribute("data-filter-id");
            });
            return filterKeys;
        }
    }, {
        key: "getPriceRange",
        value: function getPriceRange() {
            var priceRange = "";
            if (this.priceMin.value !== "") priceRange += "&priceMin=" + this.priceMin.value;
            if (this.priceMax.value !== "") priceRange += "&priceMax=" + this.priceMax.value;
            return priceRange;
        }
    }, {
        key: "getUrl",
        value: function getUrl() {
            var url = "/part/startpage/hotels?sortBy=" + this.sortBy + "&sortOrder=" + this.sortOrder + "&filters=" + this.getActiveFilters() + "&" + this.getPriceRange();
            return url;
        }
    }, {
        key: "loadHotels",
        value: function loadHotels() {
            var _this4 = this;

            var url = this.getUrl();

            (0, _Ajax.makeRequest)('GET', url).then(function (data) {
                _this4.displayResult(data);
            }).catch(function (err) {
                console.error('Ouch, there was an error! ', url, err);
            });
        }
    }, {
        key: "displayResult",
        value: function displayResult(data) {
            if (data == null) return;

            this.hotelListResult.innerHTML = data;
        }
    }]);

    return FilterHotels;
}();

exports.default = FilterHotels;
//# sourceMappingURL=filter-hotels.js.map
