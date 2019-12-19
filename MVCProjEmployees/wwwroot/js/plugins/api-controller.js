import constants from '../utils/constants.js';
import { ENTITY_TYPES, QUERY_TYPES } from '../utils/enums.js';

let controller = (function () {
    let instance;

    function ApiController(values) {
        this.options = values || {};
        this.options.API_URL = this.options.API_URL || constants.API_URL;
    }

    ApiController.prototype.loadHtmlPage = async function (resource) {
        const self = this;

        return new Promise((resolve) => {
            $(".main-list-page").load(`${self.options.API_URL}/${resource}`, () => {
                resolve();
            });
        });
    }

    ApiController.prototype.getDepartmentEmployeesByEmployeeNumber = async function (pathParameters) {
        let computedUrl = `${this.options.API_URL}/api/departments/employees/employee-departments`;

        pathParameters = pathParameters || {};

        if (!pathParameters.employeeNumber) return;

        computedUrl += `/${pathParameters.employeeNumber}`;

        const resultedData = await $.ajax({
            url: computedUrl,
            contentType: 'application/json; charset=UTF-8',
            method: 'GET'
        });

        return resultedData;
    }

    ApiController.prototype.getTitlesByEmployeeNumber = async function (pathParameters) {
        let computedUrl = `${this.options.API_URL}/api/employees`;

        pathParameters = pathParameters || {};

        if (!pathParameters.employeeNumber) return;

        computedUrl += `/${pathParameters.employeeNumber}/titles`;

        const resultedData = await $.ajax({
            url: computedUrl,
            contentType: 'application/json; charset=UTF-8',
            method: 'GET'
        });

        return resultedData;
    }

    ApiController.prototype.getSalariesByEmployeeNumber = async function (pathParameters) {
        let computedUrl = `${this.options.API_URL}/api/employees`;

        pathParameters = pathParameters || {};

        if (!pathParameters.employeeNumber) return;

        computedUrl += `/${pathParameters.employeeNumber}/salaries`;

        const resultedData = await $.ajax({
            url: computedUrl,
            contentType: 'application/json; charset=UTF-8',
            method: 'GET'
        });

        return resultedData;
    }

    ApiController.prototype.getOneEmployee = async function (pathParameters) {
        let computedUrl = `${this.options.API_URL}/api/employees`;

        pathParameters = pathParameters || {};

        if (!pathParameters.employeeNumber) return;

        computedUrl += `/${pathParameters.employeeNumber}`;

        const resultedData = await $.ajax({
            url: computedUrl,
            contentType: 'application/json; charset=UTF-8',
            method: 'GET'
        });

        return resultedData;
    }

    ApiController.prototype.getAllEmployees = async function (queryParameters) {
        let computedUrl = `${this.options.API_URL}/api/employees`;

        queryParameters = queryParameters || {};

        queryParameters.page = queryParameters.page || 0;
        queryParameters.pageSize = queryParameters.pageSize || 15;
        queryParameters.filter = queryParameters.filter || '';
        queryParameters.sort = queryParameters.sort || '';

        computedUrl += `?page=${queryParameters.page}&pageSize=${queryParameters.pageSize}&filter=${queryParameters.filter}&sort=${queryParameters.sort}`;

        const resultedData = await $.ajax({
            url: computedUrl,
            contentType: 'application/json; charset=UTF-8',
            method: 'GET'
        });

        window.sessionStorage.setItem(constants.LAST_QUERY_STATE, QUERY_TYPES.NORMAL_QUERY);

        return resultedData;
    }

    ApiController.prototype.getAllDepartments = async function (queryParameters) {
        let computedUrl = `${this.options.API_URL}/api/departments`;

        queryParameters = queryParameters || {};

        queryParameters.page = queryParameters.page || 0;
        queryParameters.pageSize = queryParameters.pageSize || 15;
        queryParameters.filter = queryParameters.filter || '';
        queryParameters.sort = queryParameters.sort || '';

        computedUrl += `?page=${queryParameters.page}&pageSize=${queryParameters.pageSize}&filter=${queryParameters.filter}&sort=${queryParameters.sort}`;

        const resultedData = await $.ajax({
            url: computedUrl,
            contentType: 'application/json; charset=UTF-8',
            method: 'GET'
        });

        window.sessionStorage.setItem(constants.LAST_QUERY_STATE, QUERY_TYPES.NORMAL_QUERY);

        return resultedData;
    }

    ApiController.prototype.getAllSalaries = async function (queryParameters) {
        let computedUrl = `${this.options.API_URL}/api/employees/salaries`;

        queryParameters = queryParameters || {};

        queryParameters.page = queryParameters.page || 0;
        queryParameters.pageSize = queryParameters.pageSize || 15;
        queryParameters.filter = queryParameters.filter || '';
        queryParameters.sort = queryParameters.sort || '';

        computedUrl += `?page=${queryParameters.page}&pageSize=${queryParameters.pageSize}&filter=${queryParameters.filter}&sort=${queryParameters.sort}`;

        const resultedData = await $.ajax({
            url: computedUrl,
            contentType: 'application/json; charset=UTF-8',
            method: 'GET'
        });

        window.sessionStorage.setItem(constants.LAST_QUERY_STATE, QUERY_TYPES.NORMAL_QUERY);

        return resultedData;
    }

    ApiController.prototype.searchEmployee = async function (queryParameters, searchString) {
        let computedUrl = `${this.options.API_URL}/api/employees/search`;

        queryParameters = queryParameters || {};

        queryParameters.page = queryParameters.page || 0;
        queryParameters.pageSize = queryParameters.pageSize || 15;
        queryParameters.filter = queryParameters.filter || '';
        queryParameters.sort = queryParameters.sort || '';

        computedUrl += `?page=${queryParameters.page}&pageSize=${queryParameters.pageSize}&filter=${queryParameters.filter}&sort=${queryParameters.sort}&searchString=${searchString}`;

        const resultedData = await $.ajax({
            url: computedUrl,
            contentType: 'application/json; charset=UTF-8',
            method: 'GET'
        });

        window.sessionStorage.setItem(constants.LAST_QUERY_STATE, QUERY_TYPES.SEARCH_QUERY);

        return resultedData;
    }

    ApiController.prototype.searchDepartment = async function (queryParameters, searchString) {
        let computedUrl = `${this.options.API_URL}/api/departments/search`;

        queryParameters = queryParameters || {};

        queryParameters.page = queryParameters.page || 0;
        queryParameters.pageSize = queryParameters.pageSize || 15;
        queryParameters.filter = queryParameters.filter || '';
        queryParameters.sort = queryParameters.sort || '';

        computedUrl += `?page=${queryParameters.page}&pageSize=${queryParameters.pageSize}&filter=${queryParameters.filter}&sort=${queryParameters.sort}&searchString=${searchString}`;

        const resultedData = await $.ajax({
            url: computedUrl,
            contentType: 'application/json; charset=UTF-8',
            method: 'GET'
        });

        window.sessionStorage.setItem(constants.LAST_QUERY_STATE, QUERY_TYPES.SEARCH_QUERY);

        return resultedData;
    }

    ApiController.prototype.searchSalary = async function (queryParameters, searchString) {
        let computedUrl = `${this.options.API_URL}/api/employees/salaries/search`;

        queryParameters = queryParameters || {};

        queryParameters.page = queryParameters.page || 0;
        queryParameters.pageSize = queryParameters.pageSize || 15;
        queryParameters.filter = queryParameters.filter || '';
        queryParameters.sort = queryParameters.sort || '';

        computedUrl += `?page=${queryParameters.page}&pageSize=${queryParameters.pageSize}&filter=${queryParameters.filter}&sort=${queryParameters.sort}&searchString=${searchString}`;

        const resultedData = await $.ajax({
            url: computedUrl,
            contentType: 'application/json; charset=UTF-8',
            method: 'GET'
        });

        window.sessionStorage.setItem(constants.LAST_QUERY_STATE, QUERY_TYPES.SEARCH_QUERY);

        return resultedData;
    }

    ApiController.prototype.createEntity = async function (entityType, body) {
        let requestUrl;
        switch (parseInt(entityType)) {
            case ENTITY_TYPES.EMPLOYEE:
                requestUrl = `${this.options.API_URL}/api/employees`;
                break;
            case ENTITY_TYPES.DEPARTMENT:
                requestUrl = `${this.options.API_URL}/api/departments`;
                break;
            case ENTITY_TYPES.SALARY:
                requestUrl = `${this.options.API_URL}/api/employees/salaries`;
                break;
            default:
                throw new Error("Invalid entity type.");
        }

        await $.ajax({
            url: requestUrl,
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            data: JSON.stringify(body),
            method: 'POST'
        });
    }

    ApiController.prototype.deleteEntity = async function (entityType, keyObject) {
        if (!keyObject) return;

        let requestUrl;

        switch (parseInt(entityType)) {
            case ENTITY_TYPES.EMPLOYEE:
                if (!keyObject.employeeNumber || keyObject.employeeNumber < 0) throw new Error("Invalid key object.");
                requestUrl = `${this.options.API_URL}/api/employees/${keyObject.employeeNumber}`;
                break;
            case ENTITY_TYPES.DEPARTMENT:
                if (!keyObject.departmentNumber || keyObject.departmentNumber === '') throw new Error("Invalid key object.");
                requestUrl = `${this.options.API_URL}/api/departments/${keyObject.departmentNumber}`;
                break;
            case ENTITY_TYPES.SALARY:
                if (!keyObject.employeeNumber || keyObject.employeeNumber < 0) throw new Error("Invalid key object.");
                if (!keyObject.fromDate) throw new Error("Invalid key object.");
                requestUrl = `${this.options.API_URL}/api/employees/${keyObject.employeeNumber}/salaries/startDate/${keyObject.fromDate}`;
                break;
            default:
                throw new Error("Invalid entity type.");
        }

        await $.ajax({
            url: requestUrl,
            contentType: 'application/json; charset=UTF-8',
            method: 'DELETE'
        });
    }

    return {
        getInstance: function (values) {
            if (instance === undefined) {
                instance = new ApiController(values);
            }

            return instance;
        }
    }
})();

export default controller;