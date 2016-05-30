import {makeRequest} from './Ajax.js';

class FilterHotels {
    
    constructor() {
        this.filterSection = document.querySelector(".filter-section");
        this.hotelList = document.querySelector(".hotel-list");
        this.hotelListResult = document.querySelector(".hotel-list__result");
        this.sortBy = "";
        this.sortOrder = "";
        this.setupSortButtons();
        this.setupFilterButtons();
        this.setupPriceRange();
    }

    setupFilterButtons() {
        const filterButtons = Array.prototype.slice.call(this.filterSection.querySelectorAll(".filter-button"));
        filterButtons.forEach(btn => btn.addEventListener("click", (e) => this.toggleFilter(e)));
    }

    setupPriceRange() {
        this.priceMin = this.filterSection.querySelector("input[name='priceMin']");
        this.priceMax = this.filterSection.querySelector("input[name='priceMax']");
        this.priceMin.addEventListener("keyup", () => this.loadHotels());
        this.priceMax.addEventListener("keyup", () => this.loadHotels());
    }

    setupSortButtons() {
        this.sortButtons = Array.prototype.slice.call(this.hotelList.querySelectorAll(".sort-button"));
        this.sortButtons.forEach(btn => btn.addEventListener("click", (e) => this.sortList(e)));
    }

    toggleFilter(e) {
        if (e.target.getAttribute("data-filter-active") === "on") {
            e.target.setAttribute("data-filter-active", "off");
            e.target.classList.remove("filter-button--active");
        } else {
            e.target.setAttribute("data-filter-active", "on");
            e.target.classList.add("filter-button--active");
        }
        this.loadHotels();
    }

    sortList(e) {
        this.sortBy = e.target.getAttribute("data-sort-by");
        const sortOrder = e.target.getAttribute("data-sort-order");
        this.sortButtons.forEach(x => x.setAttribute("data-sort-order", ""));

        if (sortOrder === "asc") {
            this.sortOrder = "desc";
            e.target.setAttribute("data-sort-order", "desc");
        } else {
            e.target.setAttribute("data-sort-order", "asc");
            this.sortOrder = "asc";
        }
        this.loadHotels();
    }

    getActiveFilters() {
        const activeFilters = Array.prototype.slice.call(this.filterSection.querySelectorAll("[data-filter-active='on']"));
        const filterKeys = activeFilters.map((x) => { return x.getAttribute("data-filter-id") });
        return filterKeys;
    }

    getPriceRange() {
        let priceRange = "";
        if (this.priceMin.value !== "")
            priceRange += `&priceMin=${this.priceMin.value}`;
        if (this.priceMax.value !== "")
            priceRange += `&priceMax=${this.priceMax.value}`;
        return priceRange;
    }


    getUrl() {
        const url = `/part/startpage/hotels?sortBy=${this.sortBy}&sortOrder=${this.sortOrder}&filters=${this.getActiveFilters()}&${this.getPriceRange()}`;
        return url;
    }

    loadHotels() {
        const url = this.getUrl();

        makeRequest('GET', url)
            .then((data) => {
                this.displayResult(data);
            })
            .catch((err) => {
                console.error('Ouch, there was an error! ', url, err);
            });
    }

    displayResult(data) {
        if (data == null)
            return;

        this.hotelListResult.innerHTML = data;
    }
}

export default FilterHotels;
