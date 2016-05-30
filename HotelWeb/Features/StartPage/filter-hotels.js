import {makeRequest} from './Ajax.js';

class FilterHotels {
    
    constructor() {
        this.hotelFilterSection = document.querySelector(".hotel-filter-section");
        this.sortBy = "SortByName";
        this.sortOrder = "asc";
        this.init();
    }

    init() {
        this.setupSortButtons();
        this.setupFilterButtons();
        this.setupPriceRange();
    }

    setupFilterButtons() {
        const filterButtons = Array.prototype.slice.call(this.hotelFilterSection.querySelectorAll(".filter-button"));
        filterButtons.forEach(btn => btn.addEventListener("click", (e) => this.toggleFilter(e)));
    }

    setupPriceRange() {
        this.priceMin = this.hotelFilterSection.querySelector("input[name='priceMin']");
        this.priceMax = this.hotelFilterSection.querySelector("input[name='priceMax']");
        this.priceMinValue = this.hotelFilterSection.querySelector(".priceMinValue");
        this.priceMaxValue = this.hotelFilterSection.querySelector(".priceMaxValue");

        this.priceMaxValue.textContent = this.priceMax.value;
        this.priceMinValue.textContent = this.priceMin.value;

        this.priceMin.addEventListener("change", (e) =>
        {
            this.priceMinValue.textContent = e.target.value;
            this.loadHotels();
        });
        this.priceMax.addEventListener("change", (e) =>
        {
            this.priceMaxValue.textContent = e.target.value;
            this.loadHotels();
        });

    }

    setupSortButtons() {
        this.sortButtons = Array.prototype.slice.call(this.hotelFilterSection.querySelectorAll(".sort-button"));
        this.sortButtons.forEach(btn => btn.addEventListener("click", (e) => this.sortList(e)));
    }

    toggleFilter(e) {
        if (e.target.getAttribute("data-filter-active") === "true") {
            e.target.setAttribute("data-filter-active", "false");
        } else {
            e.target.setAttribute("data-filter-active", "true");
        }
        this.loadHotels();
    }

    sortList(e) {
        this.sortBy = e.target.getAttribute("data-sort-by");
        this.sortButtons.forEach(x => x.setAttribute("data-sort-order", ""));
        this.sortOrder = this.sortOrder === "asc" ? "desc" : "asc";
        e.target.setAttribute("data-sort-order", this.sortOrder);
        this.loadHotels();
    }

    getActiveFilters(filterGroup) {
        const activeFilters = Array.prototype.slice.call(this.hotelFilterSection.querySelectorAll(`[data-filter-active='true'][data-filter-group='${filterGroup}']`));
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
        const url = `/part/startpage/hotels?sortBy=${this.sortBy}&sortOrder=${this.sortOrder}&countryFilters=${this.getActiveFilters("countryFilters")}&${this.getPriceRange()}`;
        return url;
    }

    loadHotels() {
        const url = this.getUrl();

        makeRequest('GET', url)
            .then((data) => {
                this.displayResult(data);
            })
            .then(() => this.init())
            .catch((err) => {
                console.error('Ouch, there was an error! ', url, err);
            });
    }

    displayResult(data) {
        this.hotelFilterSection.innerHTML = data;
    }
}

export default FilterHotels;
