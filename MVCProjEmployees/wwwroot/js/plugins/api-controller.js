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

    ApiController.prototype.getEntityByIdentifier = async function (entityType, queryParameters, uniqueIdentifiers) {
        let requestUrl;

        uniqueIdentifiers = uniqueIdentifiers || {};

        queryParameters = queryParameters || {};

        queryParameters.page = queryParameters.page || constants.QUERY_PARAMETER_DEFAULT_PAGE;
        queryParameters.pageSize = queryParameters.pageSize || constants.QUERY_PARAMETER_DEFAULT_PAGE_SIZE;
        queryParameters.filter = queryParameters.filter || constants.QUERY_PARAMETER_DEFAULT_FILTER;
        queryParameters.sort = queryParameters.sort || constants.QUERY_PARAMETER_DEFAULT_SORT;

        switch (parseInt(entityType)) {
            case ENTITY_TYPES.EMPLOYEE:
                if (uniqueIdentifiers.employeeNumber && uniqueIdentifiers.employeeNumber > 0) {
                    requestUrl = `${this.options.API_URL}/api/employees/${uniqueIdentifiers.employeeNumber}`;
                }

                break;
            case ENTITY_TYPES.DEPARTMENT_EMPLOYEE:
                if (uniqueIdentifiers.employeeNumber && uniqueIdentifiers.employeeNumber > 0) {
                    requestUrl = `${this.options.API_URL}/api/departments/employees/employee-departments/${uniqueIdentifiers.employeeNumber}`;
                }

                break;
            case ENTITY_TYPES.DEPARTMENT_MANAGER:
                if (uniqueIdentifiers.employeeNumber && uniqueIdentifiers.employeeNumber > 0) {
                    requestUrl = `${this.options.API_URL}/api/departments/managers/manager-departments/${uniqueIdentifiers.employeeNumber}`;
                }

                break;
            case ENTITY_TYPES.TITLE:
                if (uniqueIdentifiers.employeeNumber && uniqueIdentifiers.employeeNumber > 0) {
                    requestUrl = `${this.options.API_URL}/api/employees/${uniqueIdentifiers.employeeNumber}/titles`;
                }

                break;
            case ENTITY_TYPES.SALARY:
                if (uniqueIdentifiers.employeeNumber && uniqueIdentifiers.employeeNumber > 0) {
                    requestUrl = `${this.options.API_URL}/api/employees/${uniqueIdentifiers.employeeNumber}/salaries`;
                }

                break;
            default:
                throw new Error('Invalid entity type.');
                break;
        }

        if (!requestUrl) return;

        requestUrl += `?page=${queryParameters.page}&pageSize=${queryParameters.pageSize}&filter=${queryParameters.filter}&sort=${queryParameters.sort}`;

        const resultedData = await $.ajax({
            url: requestUrl,
            contentType: 'application/json; charset=UTF-8',
            method: 'GET'
        });

        return resultedData;
    }

    ApiController.prototype.getEntity = async function (entityType, queryParameters) {
        let requestUrl;
        
        queryParameters = queryParameters || {};

        queryParameters.page = queryParameters.page || constants.QUERY_PARAMETER_DEFAULT_PAGE;
        queryParameters.pageSize = queryParameters.pageSize || constants.QUERY_PARAMETER_DEFAULT_PAGE_SIZE;
        queryParameters.filter = queryParameters.filter || constants.QUERY_PARAMETER_DEFAULT_FILTER;
        queryParameters.sort = queryParameters.sort || constants.QUERY_PARAMETER_DEFAULT_SORT;

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
                throw new Error('Invalid entity type.');
        }

        requestUrl += `?page=${queryParameters.page}&pageSize=${queryParameters.pageSize}&filter=${queryParameters.filter}&sort=${queryParameters.sort}`;

        const resultedData = await $.ajax({
            url: requestUrl,
            contentType: 'application/json; charset=UTF-8',
            method: 'GET'
        });

        window.sessionStorage.setItem(constants.LAST_QUERY_STATE, QUERY_TYPES.NORMAL_QUERY);

        return resultedData;
    }

    ApiController.prototype.searchEntity = async function (entityType, queryParameters, searchString) {
        let requestUrl;

        queryParameters = queryParameters || {};

        queryParameters.page = queryParameters.page || constants.QUERY_PARAMETER_DEFAULT_PAGE;
        queryParameters.pageSize = queryParameters.pageSize || constants.QUERY_PARAMETER_DEFAULT_PAGE_SIZE;
        queryParameters.filter = queryParameters.filter || constants.QUERY_PARAMETER_DEFAULT_FILTER;
        queryParameters.sort = queryParameters.sort || constants.QUERY_PARAMETER_DEFAULT_SORT;

        switch (parseInt(entityType)) {
            case ENTITY_TYPES.EMPLOYEE:
                requestUrl = `${this.options.API_URL}/api/employees/search`;
                break;
            case ENTITY_TYPES.DEPARTMENT:
                requestUrl = `${this.options.API_URL}/api/departments/search`;
                break;
            case ENTITY_TYPES.SALARY:
                requestUrl = `${this.options.API_URL}/api/employees/salaries/search`;
                break;
            default:
                throw new Error('Invalid entity type.');
        }

        requestUrl += `?page=${queryParameters.page}&pageSize=${queryParameters.pageSize}&filter=${queryParameters.filter}&sort=${queryParameters.sort}&searchString=${searchString}`;

        const resultedData = await $.ajax({
            url: requestUrl,
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
                throw new Error('Invalid entity type.');
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
                throw new Error('Invalid entity type.');
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