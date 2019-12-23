import { ENTITY_TYPES } from '../utils/enums.js';
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

    function addLeftOffsetHandler(context) {
        $(".table-plugin-offset-left").click(async () => {
            const pageCount = window.sessionStorage.getItem(constants.STORAGE_PAGE_COUNTER);
            const currentFilter = window.sessionStorage.getItem(constants.CURRENT_APPLIED_FILTER);
            const currentSortOrder = window.sessionStorage.getItem(constants.CURRENT_APPLIED_SORT_ORDER);

            let queryParameters = {};

            queryParameters.pageSize = $(".returned-records-dropdown").val();
            queryParameters.page = parseInt(pageCount) - 1;
            queryParameters.filter = currentFilter;
            queryParameters.sort = currentSortOrder;

            if (queryParameters.page >= 0) {
                $(".plugin-added-table-ui tbody").empty();

                createTableBody(context);

                $(".table-plugin-counter-text").text(queryParameters.page + 1);
            }
        });
    }

    function addTableListeners(context) {
        addLeftOffsetHandler(context);
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

                        let tableRow = `
                        <tr>
                            <td>${departmentName}</td>
                            <td>${departmentNumber}</td>
                            <td>${deptEmp.fromDate.substring(0, 10)}</td>
                            <td>${deptEmp.toDate.substring(0, 10)}</td>
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
                        let tableRow = `
                        <tr>
                            <td>${empTitle.title}</td>
                            <td>${empTitle.fromDate.substring(0, 10)}</td>
                            <td>${empTitle.toDate.substring(0, 10)}</td>
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

                        let tableRow = `
                        <tr>
                            <td>${employeeName}</td>
                            <td>${employeeNumber}</td>
                            <td>${departmentManagers.fromDate.substring(0, 10)}</td>
                            <td>${departmentManagers.toDate.substring(0, 10)}</td>
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
                        let tableRow = `
                        <tr>
                            <td>${empSalary.salary}</td>
                            <td>${empSalary.fromDate.substring(0, 10)}</td>
                            <td>${empSalary.toDate.substring(0, 10)}</td>
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

        return rows;
    }

    async function createTableBody(context) {
        let data = await apiController.getEntityByIdentifier(context.options.LOADED_ENTITY, null, context.options.LOADED_ENTITY_IDENTIFIERS);
        return addEntityRows(context.options.LOADED_ENTITY, data);
    }

    async function createHtmlElement(context) {
        let tableHeading = createTableHead(context);
        let tableBody = await createTableBody(context);

        if (!tableBody || tableBody === '') tableBody = `<tr><td colspan="100%"><p>No entries found.</p></td></tr>`;

        const tableHtml = `
        <table class="table plugin-added-table-ui">
            <thead>${tableHeading}</thead>
            <tbody>${tableBody}</tbody>
        </table>
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