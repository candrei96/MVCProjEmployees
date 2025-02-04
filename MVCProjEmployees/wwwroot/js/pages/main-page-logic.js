﻿import ApiController from '../plugins/api-controller.js';
import constants from '../utils/constants.js';
import { ENTITY_TYPES, QUERY_TYPES } from '../utils/enums.js';
import { calculateAge } from '../utils/utilityKit.js';
import { loadEmployeePage } from './employeePage.js';
import { loadDepartmentPage } from './departmentPage.js';

const apiController = ApiController.getInstance();
let timeout = null;

function searchBarInputHandler() {
    $(".input-search").on('input', () => {
        if (timeout) {
            clearTimeout(timeout);
        }
        timeout = setTimeout(async () => {
            await searchElement();
        }, 300);
    });
}

function documentEventHandler() {
    $(document).ready(() => {
        $(document).on("click", (event) => {
            if (!event.target.classList.contains("options-crumble-menu-option")
                && !event.target.classList.contains("btn-options-entity")) {
                $(".options-crumble-menu-option").hide();

                if ($(".btn-options-entity").hasClass("active-more-options-dotted-btn")) {
                    $(".btn-options-entity")
                        .removeClass("active-more-options-dotted-btn");
                }
            }

            if (event.target.classList.contains("modal")) {
                closeModals();
            }
        });
    });
}

function handleNavbarDesignClick(context) {
    $(".active-navbar-item .navbar-item-text-active")
        .removeClass("navbar-item-text-active");
    $(".active-navbar-item")
        .removeClass("active-navbar-item")

    $(context)
        .addClass("active-navbar-item")
    $(".active-navbar-item .navbar-item-text")
        .addClass("navbar-item-text-active");
}

async function handleNavbarItemClick(id) {
    unloadLoadedPageLogic();
    let data;
    let loadedEntity = window.sessionStorage.getItem(constants.STORAGE_SELECTED_ENTITY_KEY);

    switch (id) {
        case constants.EMPLOYEE_TAB_ID:
            if (loadedEntity === ENTITY_TYPES.EMPLOYEE) break;
            data = await apiController.getEntity(ENTITY_TYPES.EMPLOYEE);
            await apiController.loadHtmlPage(constants.EMPLOYEE_TAB_RESOURCE);

            loadedEntity = ENTITY_TYPES.EMPLOYEE;
            break;
        case constants.DEPARTMENT_TAB_ID:
            if (loadedEntity === ENTITY_TYPES.DEPARTMENT) break;
            data = await apiController.getEntity(ENTITY_TYPES.DEPARTMENT);
            await apiController.loadHtmlPage(constants.DEPARTMENT_TAB_RESOURCE);

            loadedEntity = ENTITY_TYPES.DEPARTMENT;
            break;
        case constants.SALARY_TAB_ID:
            if (loadedEntity === ENTITY_TYPES.SALARY) break;
            data = await apiController.getEntity(ENTITY_TYPES.SALARY);
            await apiController.loadHtmlPage(constants.SALARY_TAB_RESOURCE);

            loadedEntity = ENTITY_TYPES.SALARY;
            break;
        default:
            loadedEntity = ENTITY_TYPES.INVALID_ENTITY;
            break;
    }

    initLoadedPageLogic();
    window.sessionStorage.setItem(constants.STORAGE_SELECTED_ENTITY_KEY, loadedEntity);
    window.sessionStorage.setItem(constants.STORAGE_PAGE_COUNTER, 0);

    if (data) {
        addEntityElement(loadedEntity, data);
    }

    addTableSortHandlers();
}

function addTableSortHandlers() {
    let columns = $(".main-body-page-content .main-body-page-table .main-body-table-header .table-column-filter").toArray();

    columns.forEach((column) => {
        column.addEventListener("click", async function () {
            const filterClass = this.classList.value.split(" ")[1];
            const filter = filterClass.split("-")[1];

            const loadedEntity = window.sessionStorage.getItem(constants.STORAGE_SELECTED_ENTITY_KEY);
            const pageCount = window.sessionStorage.getItem(constants.STORAGE_PAGE_COUNTER);
            const queryState = parseInt(window.sessionStorage.getItem(constants.LAST_QUERY_STATE));
            const searchString = $(".input-search").val();

            let data;

            let queryParameters = {};

            queryParameters.pageSize = $(".returned-records-dropdown").val();
            queryParameters.page = pageCount;
            queryParameters.filter = filter;

            $(".sort-btn").toArray().forEach((element) => {
                if (!element.parentElement.classList.contains(filterClass)) {
                    let parentClass = element.parentElement.classList.value.split(" ")[1];
                    $(`.${parentClass} .sort-btn`).addClass("half-spin-animation");
                    $(`.${parentClass} .sort-btn`).removeClass("half-spin-backwards-animation");
                }
            });

            if ($(`.${filterClass} .sort-btn`).hasClass("half-spin-animation")) {
                queryParameters.sort = 'asc';
                $(`.${filterClass} .sort-btn`).addClass("half-spin-backwards-animation");
                $(`.${filterClass} .sort-btn`).removeClass("half-spin-animation");
            } else if ($(`.${filterClass} .sort-btn`).hasClass("half-spin-backwards-animation")) {
                queryParameters.sort = 'desc';
                $(`.${filterClass} .sort-btn`).addClass("half-spin-animation");
                $(`.${filterClass} .sort-btn`).removeClass("half-spin-backwards-animation");
            } else {
                queryParameters.sort = 'asc';
                $(`.${filterClass} .sort-btn`).addClass("half-spin-backwards-animation");
            }

            $(".main-body-page-table tbody").empty();

            if (queryState === QUERY_TYPES.NORMAL_QUERY) {
                data = await apiController.getEntity(loadedEntity, queryParameters);;
                addEntityElement(loadedEntity, data);
            } else if (queryState === QUERY_TYPES.SEARCH_QUERY) {
                data = await apiController.searchEntity(loadedEntity, queryParameters, searchString);
                addEntityElement(loadedEntity, data);
            } else {
                throw new Error("Invalid query state.");
            }

            window.sessionStorage.setItem(constants.CURRENT_APPLIED_FILTER, queryParameters.filter);
            window.sessionStorage.setItem(constants.CURRENT_APPLIED_SORT_ORDER, queryParameters.sort);
        }, false);
    });
}

function dropdownChangeHandler() {
    $(".returned-records-dropdown").change(async function () {
        let queryParameters = {};
        let data;

        const loadedEntity = window.sessionStorage.getItem(constants.STORAGE_SELECTED_ENTITY_KEY);
        const queryState = parseInt(window.sessionStorage.getItem(constants.LAST_QUERY_STATE));
        const pageCount = window.sessionStorage.getItem(constants.STORAGE_PAGE_COUNTER);
        const currentFilter = window.sessionStorage.getItem(constants.CURRENT_APPLIED_FILTER);
        const currentSortOrder = window.sessionStorage.getItem(constants.CURRENT_APPLIED_SORT_ORDER);
        const searchString = $(".input-search").val();

        queryParameters.pageSize = $(".returned-records-dropdown").val();
        queryParameters.page = pageCount;
        queryParameters.filter = currentFilter;
        queryParameters.sort = currentSortOrder;

        $(".main-body-page-table tbody").empty();

        if (queryState === QUERY_TYPES.NORMAL_QUERY) {
            data = await apiController.getEntity(loadedEntity, queryParameters);;
            addEntityElement(loadedEntity, data);
        } else if (queryState === QUERY_TYPES.SEARCH_QUERY) {
            data = await apiController.searchEntity(loadedEntity, queryParameters, searchString);
            addEntityElement(loadedEntity, data);
        } else {
            throw new Error("Invalid query state.");
        }
    });
}

function pageOffsetClickHandlers() {
    $(".page-offset-left").click(async () => {
        const loadedEntity = window.sessionStorage.getItem(constants.STORAGE_SELECTED_ENTITY_KEY);
        const queryState = parseInt(window.sessionStorage.getItem(constants.LAST_QUERY_STATE));
        const pageCount = window.sessionStorage.getItem(constants.STORAGE_PAGE_COUNTER);
        const currentFilter = window.sessionStorage.getItem(constants.CURRENT_APPLIED_FILTER);
        const currentSortOrder = window.sessionStorage.getItem(constants.CURRENT_APPLIED_SORT_ORDER);

        const searchString = $(".input-search").val();

        let queryParameters = {};
        let data;

        queryParameters.pageSize = $(".returned-records-dropdown").val();
        queryParameters.page = parseInt(pageCount) - 1;
        queryParameters.filter = currentFilter;
        queryParameters.sort = currentSortOrder;

        if (queryParameters.page >= 0) {
            $(".main-body-page-table tbody").empty();

            if (queryState === QUERY_TYPES.NORMAL_QUERY) {
                data = await apiController.getEntity(loadedEntity, queryParameters);;
                addEntityElement(loadedEntity, data);
            } else if (queryState === QUERY_TYPES.SEARCH_QUERY) {
                data = await apiController.searchEntity(loadedEntity, queryParameters, searchString);
                addEntityElement(loadedEntity, data);
            } else {
                throw new Error("Invalid query state.");
            }

            window.sessionStorage.setItem(constants.STORAGE_PAGE_COUNTER, queryParameters.page);
            $(".page-counter-text").text(queryParameters.page + 1);
        }
    });
    $(".page-offset-right").click(async () => {
        if ($(".main-body-page-table tbody")[0].rows.length <= 0) return;

        const loadedEntity = window.sessionStorage.getItem(constants.STORAGE_SELECTED_ENTITY_KEY);
        const queryState = parseInt(window.sessionStorage.getItem(constants.LAST_QUERY_STATE));
        const pageCount = window.sessionStorage.getItem(constants.STORAGE_PAGE_COUNTER);
        const currentFilter = window.sessionStorage.getItem(constants.CURRENT_APPLIED_FILTER);
        const currentSortOrder = window.sessionStorage.getItem(constants.CURRENT_APPLIED_SORT_ORDER);

        const searchString = $(".input-search").val();

        let queryParameters = {};
        let data;

        queryParameters.pageSize = $(".returned-records-dropdown").val();
        queryParameters.page = parseInt(pageCount) + 1;
        queryParameters.filter = currentFilter;
        queryParameters.sort = currentSortOrder;

        $(".main-body-page-table tbody").empty();
        if (queryState === QUERY_TYPES.NORMAL_QUERY) {
            data = await apiController.getEntity(loadedEntity, queryParameters);;
            addEntityElement(loadedEntity, data);
        } else if (queryState === QUERY_TYPES.SEARCH_QUERY) {
            data = await apiController.searchEntity(loadedEntity, queryParameters, searchString);
            addEntityElement(loadedEntity, data);
        } else {
            throw new Error("Invalid query state.");
        }

        window.sessionStorage.setItem(constants.STORAGE_PAGE_COUNTER, queryParameters.page);
        $(".page-counter-text").text(queryParameters.page + 1);
    });
}

function addEntityElement(entityType, data) {
    data = data || [];

    $(".main-body-page-table tbody").empty();

    for (let i = 0; i < data.length; i++) {
        if (i > constants.MAXIMUM_TABLE_ROWS) break;

        $(".main-body-page-table tbody").append(`<tr id=main-table-data${i}></tr>`);

        addEntityRow(data[i], entityType, i);
    }

    $('.table-delete-checkbox').on('change', function () {
        $('.table-delete-checkbox').not(this).prop('checked', false);
    });
}

function addEntityRow(entity, entityType, index) {
    switch (parseInt(entityType)) {
        case ENTITY_TYPES.EMPLOYEE:
            addEmployeeRow(entity, index);
            break;
        case ENTITY_TYPES.DEPARTMENT:
            addDepartmentRow(entity, index);
            break;
        case ENTITY_TYPES.SALARY:
            addSalaryRow(entity, index);
            break;
        default:
            throw new Error('Invalid entity type.');
            break;
    }
}

function addEmployeeRow(employee, index) {
    let gender = employee.employeeGender === 'M' ? 'Male' : 'Female';

    $(`#main-table-data${index}`).append(`<td>${employee.firstName} ${employee.lastName}</td>`);
    $(`#main-table-data${index}`).append(`<td>${employee.employeeNumber}</td>`);
    $(`#main-table-data${index}`).append(`<td>${gender}</td>`);
    $(`#main-table-data${index}`).append(`<td>${employee.birthDate.substring(0, 10)}</td>`);
    $(`#main-table-data${index}`).append(`<td>${employee.hireDate.substring(0, 10)}</td>`);
    $(`#main-table-data${index}`).append(`<td><input class="table-delete-checkbox form-check-input" type="checkbox" /></td>`);

    let employeeNumber = employee.employeeNumber;

    $(`#main-table-data${index}`).click(async () => {
        let uniqueIdentifiers = {};

        uniqueIdentifiers.employeeNumber = employeeNumber;

        await loadEmployeePage(uniqueIdentifiers);
    });
}

function addDepartmentRow(department, index) {
    $(`#main-table-data${index}`).append(`<td>${department.departmentNumber}</td>`);
    $(`#main-table-data${index}`).append(`<td>${department.departmentName}</td>`);
    $(`#main-table-data${index}`).append(`<td><input class="table-delete-checkbox form-check-input" type="checkbox" /></td>`);

    let departmentNumber = department.departmentNumber;

    $(`#main-table-data${index}`).click(async () => {
        let uniqueIdentifiers = {};

        uniqueIdentifiers.departmentNumber = departmentNumber;

        await loadDepartmentPage(uniqueIdentifiers);
    });
}

function addSalaryRow(salary, index) {
    let untilDate = new Date(salary.toDate) >= constants.DATABASE_DEFAULT_DATE ? '-' : salary.toDate.substring(0, 10);
    let employeeName =
        salary.employee
            ? salary.employee.firstName + ' ' + salary.employee.lastName :
            '';

    $(`#main-table-data${index}`).append(`<td>${employeeName}</td>`);
    $(`#main-table-data${index}`).append(`<td>$${salary.employeeSalary}</td>`);
    $(`#main-table-data${index}`).append(`<td>${salary.fromDate.substring(0, 10)}</td>`);
    $(`#main-table-data${index}`).append(`<td>${untilDate}</td>`);
    $(`#main-table-data${index}`).append(`<td><input class="table-delete-checkbox form-check-input" type="checkbox" /></td>`);

    let employeeNumber = salary.employeeNumber;

    $(`#main-table-data${index}`).click(async () => {
        let uniqueIdentifiers = {};

        uniqueIdentifiers.employeeNumber = employeeNumber;

        await loadEmployeePage(uniqueIdentifiers);
    });
}

function addNavbarLogic() {
    $(".navbar-item").click(async function () {
        handleNavbarDesignClick(this);
        await handleNavbarItemClick(this.getAttribute("id"));
    });
}
function hideLoader() {
    $(".loader").hide();
    $(".main-body-page-content").show();
}

function showLoader() {
    $(".loader").show();
    $(".main-body-page-content").hide();
}

function crumbleButtonLoad() {
    $(".btn-options-entity").click(function () {
        if ($(".btn-options-entity")
            .hasClass("active-more-options-dotted-btn")) {
            $(".btn-options-entity")
                .removeClass("active-more-options-dotted-btn");
            $(".options-crumble-menu-option")
                .hide();
        } else {
            $(".btn-options-entity")
                .addClass("active-more-options-dotted-btn");
            $(".options-crumble-menu-option")
                .show();
        }
    });
    $(".options-crumble-menu-option").click(() => {
        let checkedCheckboxes = $('.table-delete-checkbox:checked').toArray();
        if (checkedCheckboxes.length === 0) return;

        const loadedEntity = window.sessionStorage.getItem(constants.STORAGE_SELECTED_ENTITY_KEY);

        checkedCheckboxes.forEach((checkbox) => {
            switch (parseInt(loadedEntity)) {
                case ENTITY_TYPES.EMPLOYEE:
                    $(".modal-deleted-entity-list").append(`<li>${checkbox.parentElement.parentElement.children[0].textContent}</li>`)
                    break;
                case ENTITY_TYPES.DEPARTMENT:
                    $(".modal-deleted-entity-list").append(`<li>${checkbox.parentElement.parentElement.children[1].textContent}</li>`)
                    break;
                case ENTITY_TYPES.SALARY:
                    $(".modal-deleted-entity-list")
                        .append(`<li>${checkbox.parentElement.parentElement.children[0].textContent} ${checkbox.parentElement.parentElement.children[2].textContent}</li>`)
                    break;
                default:
                    throw new Error('Invalid entity type.');
                    break;
            }
        });

        $(".modal-delete").show();
    });
}

function crumbleButtonUnload() {
    $(".btn-options-entity").off("click");
}

function unloadLoadedPageLogic() {
    showLoader();
    crumbleButtonUnload();
}

function addEntityClickHandler() {
    $(".btn-add-entity").click(() => {
        $(".modal-create").show();
    });
}

async function searchElement() {
    let queryParameters = {};

    let loadedEntity = window.sessionStorage.getItem(constants.STORAGE_SELECTED_ENTITY_KEY);
    let data;

    queryParameters.pageSize = $(".returned-records-dropdown").val();

    const searchString = $(".input-search").val();

    switch (parseInt(loadedEntity)) {
        case ENTITY_TYPES.EMPLOYEE:
            if (searchString != '') {
                data = await apiController.searchEntity(loadedEntity, queryParameters, searchString);
                addEntityElement(loadedEntity, data);
            } else {
                data = await apiController.getEntity(loadedEntity, queryParameters);;
                addEntityElement(loadedEntity, data);
            }

            break;
        case ENTITY_TYPES.DEPARTMENT:
            if (searchString != '') {
                data = await apiController.searchEntity(loadedEntity, queryParameters, searchString);
                addEntityElement(loadedEntity, data);
            } else {
                data = await apiController.getEntity(loadedEntity, queryParameters);;
                addEntityElement(loadedEntity, data);
            }

            break;
        case ENTITY_TYPES.SALARY:
            if (searchString != '') {
                data = await apiController.searchEntity(loadedEntity, queryParameters, searchString);
                addEntityElement(loadedEntity, data);
            } else {
                data = await apiController.getEntity(loadedEntity, queryParameters);;
                addEntityElement(loadedEntity, data);
            }

            break;
        default:
            throw new Error('Invalid entity type.');
            break;
    }
}

function configureModalControls() {
    $(".is-current-salary-cb").on("change", function () {
        if ($(this).prop("checked")) {
            $('.modal-input-salary-to-date').val(constants.DATABASE_STRING_DEFAULT_DATE);
            $('.modal-input-salary-to-date').prop('disabled', true);
            $('.modal-input-salary-to-date').prop('readonly', true);
        } else {
            $('.modal-input-salary-to-date').val('');
            $('.modal-input-salary-to-date').prop('disabled', false);
            $('.modal-input-salary-to-date').prop('readonly', false);
        }
    });

    $(".datepicker").datepicker({
        dateFormat: "yy-mm-dd"
    });

    $(".close").click(() => {
        $(".modal").hide();
    });
}

function isModalInputValid() {
    $(".modal-error-zone").empty();
    let isValid = true;

    $(".modal-input").toArray().forEach((input) => {
        $(input).removeClass("red-border");

        if (!$(input).val() || $(input).val().toString() === '') {
            $(".modal-error-zone").prepend("<p class='error-message'>*Input must be completed.</p>");
            $(input).addClass("red-border");
            isValid = false;
            return;
        }

        if ($(input).hasClass("modal-input-emp-birth-date")) {
            const age = calculateAge(new Date($(input).val()));

            if (age < 18) {
                $(".modal-error-zone").prepend("<p class='error-message'>*Employee must have at least 18 years.</p>");
                $(input).addClass("red-border");
                isValid = false;
                return;
            }
        }
    });

    return isValid;
}

function closeModals() {
    $(".modal").hide();
    $(".ui-datepicker").hide();

    $(".modal-input").toArray().forEach((input) => {
        $(input).val('');
    });

    $(".modal-deleted-entity-list").empty();
}

function constructEmployeeModalObject() {
    let employeeObject = {
        firstName: $(".modal-input-emp-first-name").val(),
        lastName: $(".modal-input-emp-last-name").val(),
        birthDate: $(".modal-input-emp-birth-date").val(),
        employeeGender: $(".modal-input-emp-gender").val(),
        hireDate: $(".modal-input-emp-hire-date").val()
    }

    return employeeObject;
}

function constructDepartmentModalObject() {
    let departmentObject = {
        departmentName: $(".modal-input-dept-name").val()
    }

    return departmentObject;
}

function constructSalaryModalObject() {
    let salaryObject = {
        employeeNumber: parseInt($(".modal-input-salary-empNo").val()),
        employeeSalary: parseInt($(".modal-input-salary-salary").val()),
        fromDate: $(".modal-input-salary-from-date").val(),
        toDate: $(".modal-input-salary-to-date").val()
    }

    return salaryObject;
}

function submitCreateModalFormHandler() {
    $(".btn-submit-entity").click(() => {
        const isValid = isModalInputValid();
        if (!isValid) return;

        let loadedEntity = window.sessionStorage.getItem(constants.STORAGE_SELECTED_ENTITY_KEY);
        let body;

        switch (parseInt(loadedEntity)) {
            case ENTITY_TYPES.EMPLOYEE:
                body = constructEmployeeModalObject();
                break;
            case ENTITY_TYPES.DEPARTMENT:
                body = constructDepartmentModalObject();
                break;
            case ENTITY_TYPES.SALARY:
                body = constructSalaryModalObject();
                break;
            default:
                throw new Error('Invalid entity type.');
                break;
        }

        apiController.createEntity(loadedEntity, body);
        closeModals();
    });
}

function submitDeleteModalFormHandler() {
    $(".btn-delete-entity").click(() => {
        let checkedCheckboxes = $('.table-delete-checkbox:checked').toArray();
        if (checkedCheckboxes.length === 0) return;

        const loadedEntity = window.sessionStorage.getItem(constants.STORAGE_SELECTED_ENTITY_KEY);

        checkedCheckboxes.forEach((checkbox) => {
            let keyObject = {};

            switch (parseInt(loadedEntity)) {
                case ENTITY_TYPES.EMPLOYEE:
                    keyObject.employeeNumber = parseInt(checkbox.parentElement.parentElement.children[1].textContent);
                    $(".modal-deleted-entity-list").append(`<li>${checkbox.parentElement.parentElement.children[0].textContent}</li>`)
                    break;
                case ENTITY_TYPES.DEPARTMENT:
                    keyObject.departmentNumber = checkbox.parentElement.parentElement.children[0].textContent;
                    $(".modal-deleted-entity-list").append(`<li>${checkbox.parentElement.parentElement.children[1].textContent}</li>`)
                    break;
                case ENTITY_TYPES.SALARY:
                    keyObject.employeeNumber = parseInt(checkbox.parentElement.parentElement.children[0].textContent);
                    keyObject.fromDate = checkbox.parentElement.parentElement.children[2].textContent;
                    $(".modal-deleted-entity-list")
                        .append(`<li>${checkbox.parentElement.parentElement.children[0].textContent} ${checkbox.parentElement.parentElement.children[2].textContent}</li>`)
                    break;
                default:
                    throw new Error('Invalid entity type.');
                    break;
            }

            $(checkbox.parentElement.parentElement).detach();

            apiController.deleteEntity(loadedEntity, keyObject);
            closeModals();
        })
    });
}

export function initUnloadedPageLogic() {
    addNavbarLogic();
}

export function initLoadedPageLogic() {
    documentEventHandler();
    searchBarInputHandler();
    addEntityClickHandler();
    hideLoader();
    crumbleButtonLoad();
    dropdownChangeHandler();
    pageOffsetClickHandlers();
    configureModalControls();
    submitCreateModalFormHandler();
    submitDeleteModalFormHandler();
}
