﻿import { ENTITY_TYPES } from '../utils/enums.js';
import constants from '../utils/constants.js';
import ApiController from './api-controller.js';

const apiController = ApiController.getInstance();

let tablePlugin = (function () {
    function extendOptions(options, defaultOptions) {
        let extendedObject = {};

        const defaultOptionsKeys = Object.keys(defaultOptions);
        defaultOptionsKeys.forEach((defaultOption) => {
            extendedObject[defaultOption] = defaultOptions[defaultOption];
        });

        const optionsKeys = Object.keys(options);
        optionsKeys.forEach((option) => {
            if (Object.prototype.hasOwnProperty.call(defaultOptions, option)) {
                extendedObject[option] = options[option];
            }
        });

        return extendedObject;
    }

    function addOffsetHandlers(context) {
        $(".table-plugin-offset-left").click(async () => {
            const pageCount = $('.table-plugin-counter-text').text();

            let queryParameters = {};

            queryParameters.pageSize = $(".ui-table-plugin-dropdown").val();
            let page = parseInt(pageCount) - 1;

            queryParameters.page = page - 1;

            if (queryParameters.page >= 0) {
                $(".plugin-added-table-ui tbody").empty();

                let body = await createTableBody(context, queryParameters);

                $('.plugin-added-table-ui tbody')
                    .append(body);

                $(".table-plugin-counter-text").text(queryParameters.page + 1);
            }
        });
        $(".table-plugin-offset-right").click(async () => {
            if ($(".plugin-added-table-ui tbody")[0].rows.length <= 1) return;

            const pageCount = $('.table-plugin-counter-text').text();

            let queryParameters = {};

            queryParameters.pageSize = $(".ui-table-plugin-dropdown").val();
            let page = parseInt(pageCount) - 1;

            queryParameters.page = page + 1;

            $(".plugin-added-table-ui tbody").empty();

            let body = await createTableBody(context, queryParameters);

            $('.plugin-added-table-ui tbody')
                .append(body);

            $(".table-plugin-counter-text").text(queryParameters.page + 1);
        });
    }

    function addDropdownHandler(context) {
        $(".ui-table-plugin-dropdown").change(async function () {
            let queryParameters = {};

            const pageCount = $('.table-plugin-counter-text').text();

            queryParameters.pageSize = $(".ui-table-plugin-dropdown").val();
            queryParameters.page = pageCount - 1;

            $(".plugin-added-table-ui tbody").empty();

            let body = await createTableBody(context, queryParameters);

            $('.plugin-added-table-ui tbody')
                .append(body);
        });
    }

    function addTableListeners(context) {
        addOffsetHandlers(context);
        addDropdownHandler(context);
    }

    function createTableHead(context) {
        let tableHeading;

        switch (parseInt(context.options.LOADED_ENTITY)) {
            case ENTITY_TYPES.DEPARTMENT_EMPLOYEE:
                tableHeading = `
                <th>Department Name</th>
                <th>Department Number</th>
                <th>Start Date</th>
                <th>End Date</th>
                <th>Delete</th>
                `;
                break;
            case ENTITY_TYPES.TITLE:
                tableHeading = `
                <th>Title</th>
                <th>Start Date</th>
                <th>End Date</th>
                <th>Delete</th>
                `;
                break;
            case ENTITY_TYPES.SALARY:
                tableHeading = `
                <th>Salary Amount</th>
                <th>Start Date</th>
                <th>End Date</th>
                <th>Delete</th>
                `;
                break;
            case ENTITY_TYPES.DEPARTMENT_MANAGER:
                tableHeading = `
                <th>Manager Name</th>
                <th>Manager Number</th>
                <th>Start Date</th>
                <th>End Date</th>
                <th>Delete</th>
                `;
                break;
            default:
                throw new Error('Invalid entity type.');
        }

        return tableHeading;
    }

    function addEntityRows(entityType, entity) {
        let rows = '';

        switch (parseInt(entityType)) {
            case ENTITY_TYPES.DEPARTMENT_EMPLOYEE:
                if (entity && entity.length > 0) {
                    entity.forEach((deptEmp) => {
                        let departmentName = deptEmp.department ? deptEmp.department.departmentName : '';
                        let departmentNumber = deptEmp.department ? deptEmp.department.departmentNumber : '';
                        let untilDate = new Date(deptEmp.toDate) >= constants.DATABASE_DEFAULT_DATE ? '-' : deptEmp.toDate.substring(0, 10);

                        let tableRow = `
                        <tr>
                            <td>${departmentName}</td>
                            <td>${departmentNumber}</td>
                            <td>${deptEmp.fromDate.substring(0, 10)}</td>
                            <td>${untilDate}</td>
                            <td><input type="checkbox" /></td>
                        </tr>
                        `;

                        rows += tableRow;
                    });
                }
                break;
            case ENTITY_TYPES.TITLE:
                if (entity && entity.length > 0) {
                    entity.forEach((empTitle) => {
                        let untilDate = new Date(empTitle.toDate) >= constants.DATABASE_DEFAULT_DATE ? '-' : empTitle.toDate.substring(0, 10);

                        let tableRow = `
                        <tr>
                            <td>${empTitle.title}</td>
                            <td>${empTitle.fromDate.substring(0, 10)}</td>
                            <td>${untilDate}</td>
                            <td><input type="checkbox" /></td>
                        </tr>
                        `;
                        rows += tableRow;
                    });
                }
                break;
            case ENTITY_TYPES.DEPARTMENT_MANAGER:
                if (entity && entity.length > 0) {
                    entity.forEach((deptManager) => {
                        let employeeName = deptManager.employee ? deptManager.employee.firstName + ' ' + deptManager.employee.lastName : '';
                        let employeeNumber = deptManager.employee ? deptManager.employee.employeeNumber : '';
                        let untilDate = new Date(deptManager.toDate) >= constants.DATABASE_DEFAULT_DATE ? '-' : deptManager.toDate.substring(0, 10);

                        let tableRow = `
                        <tr>
                            <td>${employeeName}</td>
                            <td>${employeeNumber}</td>
                            <td>${departmentManagers.fromDate.substring(0, 10)}</td>
                            <td>${untilDate}</td>
                            <td><input type="checkbox" /></td>
                        </tr>
                        `;
                        rows += tableRow;
                    });
                }
                break;
            case ENTITY_TYPES.SALARY:
                if (entity && entity.length > 0) {
                    entity.forEach((empSalary) => {
                        let untilDate = new Date(empSalary.toDate) >= constants.DATABASE_DEFAULT_DATE ? '-' : empSalary.toDate.substring(0, 10);

                        let tableRow = `
                        <tr>
                            <td>${empSalary.salary}</td>
                            <td>${empSalary.fromDate.substring(0, 10)}</td>
                            <td>${untilDate}</td>
                            <td><input type="checkbox" /></td>
                        </tr>
                        `;
                        rows += tableRow;
                    });
                }
                break;
            default:
                throw new Error('Invalid entity type.');
                break;
        }

        if (!rows || rows === '') rows = `<tr><td colspan="100%"><p>No entries found.</p></td></tr>`;

        return rows;
    }

    async function createTableBody(context, qParams) {
        let data = await apiController.getEntityByIdentifier(context.options.LOADED_ENTITY, qParams, context.options.LOADED_ENTITY_IDENTIFIERS);
        return addEntityRows(context.options.LOADED_ENTITY, data);
    }

    function createTableFooter() {
        let tableFooter = `<div class="table-plugin-footer-ui">
        <button class="btn table-plugin-offset-left"><i class="fa fa-chevron-left"></i></button>
        <div class="table-plugin-page-counter btn"><span class="table-plugin-counter-text">1</span></div>
        <button class="btn table-plugin-offset-right"><i class="fa fa-chevron-right"></i></button>
        <select class="form-control ui-table-plugin-dropdown">
            <option value="5">5</option>
            <option value="10">10</option>
            <option value="15" selected>15</option>
        </select>
        <p class="select-label-text">items per page</p>
        </div>`;

        return tableFooter;
    }

    async function createHtmlElement(context) {
        let tableHeading = createTableHead(context);
        let tableBody = await createTableBody(context);
        let tableFooter = createTableFooter();

        const tableHtml = `
        <div class="plugin-added-table-ui-container animate-bottom">
            <table class="table plugin-added-table-ui">
                <thead>${tableHeading}</thead>
                <tbody>${tableBody}</tbody>
            </table>
            ${tableFooter}
        </div>
        `;

        if (!context.options.ATTACH_SELECTOR || context.options.ATTACH_SELECTOR === '') throw new Error("Invalid attach selector.");

        $(`${context.options.ATTACH_SELECTOR}`)
            .append(tableHtml);

        addTableListeners(context);
    }

    function TablePlugin(options) {
        this.defaultOptions = {
            ATTACH_SELECTOR: '',
            LOADED_ENTITY: -1,
            LOADED_ENTITY_IDENTIFIERS: {}
        };

        this.options = extendOptions(options, this.defaultOptions);
    }

    async function initPlugin(options) {
        const tablePlugin = new TablePlugin(options);

        await createHtmlElement(tablePlugin);
    }

    return initPlugin;
})();

export default tablePlugin;