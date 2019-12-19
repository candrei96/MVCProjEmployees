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

    function createTableHead(context) {
        let tableHeading;

        switch (context.LOADED_ENTITY) {
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

    async function createTableBody(context) {
        let tableBody;

        switch (context.LOADED_ENTITY) {
            case ENTITY_TYPES.DEPARTMENT_EMPLOYEE:
                let departmentEmployees = await apiController.getDepartmentEmployeesByEmployeeNumber(context.LOADED_ENTITY_IDENTIFIERS.employeeNumber);

                if (departmentEmployees && departmentEmployees.length > 0) {
                    departmentEmployees.forEach((deptEmp) => {
                        let departmentName = deptEmp.department ? deptEmp.department.departmentName : '';
                        let departmentNumber = deptEmp.department ? deptEmp.department.departmentNumber : '';

                        let tableRow = `
                        <tr>
                            <td>${departmentName}</td>
                            <td>${departmentNumber}</td>
                            <td>${deptEmp.fromDate.substring(0, 10)}</td>
                            <td>${deptEmp.toDate.substring(0, 10)}</td>
                            <td>Delete</td>
                        </tr>
                        `;
                        tableBody += tableRow;
                    });
                }

                break;
            case ENTITY_TYPES.TITLE:
                let employeeTitles = await apiController.getTitlesByEmployeeNumber(context.LOADED_ENTITY_IDENTIFIERS.employeeNumber);

                if (employeeTitles && employeeTitles.length > 0) {
                    employeeTitles.forEach((empTitle) => {
                        let tableRow = `
                        <tr>
                            <td>${empTitle.title}</td>
                            <td>${empTitle.fromDate.substring(0, 10)}</td>
                            <td>${empTitle.toDate.substring(0, 10)}</td>
                            <td>Delete</td>
                        </tr>
                        `;
                        tableBody += tableRow;
                    });
                }
                break;
            case ENTITY_TYPES.SALARY:
                let employeeSalaries = await apiController.getSalariesByEmployeeNumber(context.LOADED_ENTITY_IDENTIFIERS.employeeNumber);

                if (employeeSalaries && employeeSalaries.length > 0) {
                    employeeSalaries.forEach((empSalary) => {
                        let tableRow = `
                        <tr>
                            <td>${empSalary.salary}</td>
                            <td>${empSalary.fromDate.substring(0, 10)}</td>
                            <td>${empSalary.toDate.substring(0, 10)}</td>
                            <td>Delete</td>
                        </tr>
                        `;
                        tableBody += tableRow;
                    });
                }
                break;
            case ENTITY_TYPES.DEPARTMENT_MANAGER:
                tableHeading = `
                <td>Manager Name</td>
                <td>Manager Number</td>
                <td>Start Date</td>
                <td>End Date</td>
                <td>Delete</td>
                `;
                break;
            default:
                throw new Error('Invalid entity type.');
        }

        return tableBody;
    }

    async function createHtmlElement(context) {
        let tableHeading = createTableHead(context);
        let tableBody = await createTableBody(context);

        const tableHtml = `
        <table class="table">
            <thead>${tableHeading}</thead>
            <tbody>${tableBody}</tbody>
        </table>
        `;

        $(context.options.ATTACH_SELECTOR)
            .append(tableHtml);
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