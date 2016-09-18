import {makeRequest} from './Ajax.js';
import RangeSlider from "./range-slider.js";

class FilterHotels {
    
    constructor(element) {
        this.element = element;
        this.sortBy = "SortByName";
        this.sortOrder = "asc";
        this.filterLevel = 1;
        this.init();
    }

    init() {
        this.setupSortButtons();
        this.setupFilterButtons();
        this.setupRangeSliders();
    }

    setupFilterButtons() {
        const filterButtons = [...this.element.querySelectorAll(".filter-button")];
        filterButtons.forEach(btn => btn.addEventListener("click", (e) => this.toggleFilter(e)));
    }

    setupSortButtons() {
        this.sortButtons = [...this.element.querySelectorAll(".sort-button")];
        this.sortButtons.forEach(btn => btn.addEventListener("click", (e) => this.sortList(e)));
    }

    setupRangeSliders() {
        this.rangeSliders = [...this.element.querySelectorAll(".slider")];
        this.rangeSliders.forEach(slider => new RangeSlider(slider, () => this.loadHotels()));
    }

    toggleFilter(e) {
        if (e.target.getAttribute("data-filter-selected") === "true") {
            e.target.setAttribute("data-filter-selected", "false");
        } else {
            e.target.setAttribute("data-filter-selected", "true");
        }
        this.filterLevel = e.target.getAttribute("data-filter-level");
        this.loadHotels();
    }

    sortList(e) {
        this.sortBy = e.target.getAttribute("data-sort-by");
        this.sortButtons.forEach(x => x.setAttribute("data-sort-order", ""));
        this.sortOrder = this.sortOrder === "asc" ? "desc" : "asc";
        e.target.setAttribute("data-sort-order", this.sortOrder);
        this.loadHotels();
    }

    getCountryFilters() {
        const countryFilters = [...this.element.querySelectorAll("[data-filter-selected='true'][data-filter-group='countryFilters']")];
        const countryKeys = countryFilters.map((x) => { return x.getAttribute("data-filter-key") });
        return countryKeys.length > 0 ? `&countryFilters=${countryKeys}` : "";
    }

    getServiceFilters() {
        const serviceFilters = [...this.element.querySelectorAll("[data-filter-selected='true'][data-filter-group='serviceFilters']")];
        const serviceKeys = serviceFilters.map((x) => { return x.getAttribute("data-filter-key") });
        return serviceKeys.length > 0 ? `&serviceFilters=${serviceKeys}` : "";
    }

    getStarFilters() {
        const starFilters = [...this.element.querySelectorAll("[data-filter-selected='true'][data-filter-group='starFilters']")];
        const starKeys = starFilters.map((x) => { return x.getAttribute("data-filter-key") });
        return starKeys.length > 0 ? `&starFilters=${starKeys}` : "";
    }

    getRangeFilters() {
        let rangeFilters = "";
        this.rangeSliders.forEach(range => {
            const name = range.getAttribute("data-slider-name");
            const values = range.getAttribute("data-slider-values").split(',');
            const totalMin = values[0];
            const totalMax = values[values.length - 1];
            const rangeMin = range.querySelector("input[data-slider-handle='min']").value;
            const rangeMax = range.querySelector("input[data-slider-handle='max']").value;
            if (rangeMin !== totalMin)
                rangeFilters += `&${name}Min=${rangeMin}`;
            if (rangeMax !== totalMax)
                rangeFilters += `&${name}Max=${rangeMax}`;
        });
        return rangeFilters;
    }

    getResultCount() {
        const count = parseInt(document.querySelector(".hotel-list__count").getAttribute("data-value"));
        return isNaN(count) ? 0 : count;
    }

    getUrl() {
        const url = `/part/startpage/hotels?sortBy=${this.sortBy}&sortOrder=${this.sortOrder}${this.getCountryFilters()}${this.getServiceFilters()}${this.getStarFilters()}${this.getRangeFilters()}`;
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
        this.element.innerHTML = data;
    }
}

export default FilterHotels;
