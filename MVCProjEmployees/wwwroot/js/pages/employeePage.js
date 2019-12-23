import ApiController from '../plugins/api-controller.js';
import constants from '../utils/constants.js';

import tablePlugin from '../plugins/table.js';
import { ENTITY_TYPES } from '../utils/enums.js';

const apiController = ApiController.getInstance();

function navigationMenuItemClickHandler() {
    $(".employee-page .employee-page-header .employee-page-navigation .employee-nav-item")
        .click(function () {
            $(".employee-page .employee-page-header .employee-page-navigation .employee-nav-item.employee-nav-item-active")
                .removeClass("employee-nav-item-active");
            $(this).addClass("employee-nav-item-active");

            switch ($(this).attr('id')) {
                case constants.EMPLOYEE_PAGE_NAV_DETAILS:
                    $(".employee-page-content").toArray().forEach((div) => $(div).hide());
                    $(".employee-page-content-details").show();
                    $(".btn-employee-entity-action")
                        .empty()
                        .append("<span>Delete Employee</span>");
                    $(".btn-trash-delete-entity").hide();
                    break;
                case constants.EMPLOYEE_PAGE_NAV_DEPARTMENTS:
                    $(".employee-page-content").toArray().forEach((div) => $(div).hide());
                    $(".employee-page-content-departments").show();
                    $(".btn-employee-entity-action")
                        .empty()
                        .append("<span>Add Department</span>");
                    $(".btn-trash-delete-entity").show();
                    break;
                case constants.EMPLOYEE_PAGE_NAV_TITLES:
                    $(".employee-page-content").toArray().forEach((div) => $(div).hide());
                    $(".employee-page-content-titles").show();
                    $(".btn-employee-entity-action")
                        .empty()
                        .append("<span>Add Title</span>");
                    $(".btn-trash-delete-entity").show();
                    break;
                case constants.EMPLOYEE_PAGE_NAV_SALARIES:
                    $(".employee-page-content").toArray().forEach((div) => $(div).hide());
                    $(".employee-page-content-salaries").show();
                    $(".btn-employee-entity-action")
                        .empty()
                        .append("<span>Add Salary</span>");
                    $(".btn-trash-delete-entity").show();
                    break;
                default:
                    throw new Error('Invalid id.');
                    break;
            }
        });

    initEmployeePageListeners();
}

async function loadEmployeeDetails(uniqueIdentifiers) {
    const employee = await apiController.getEntityByIdentifier(ENTITY_TYPES.EMPLOYEE, null, uniqueIdentifiers);

    if (!employee) return;

    let gender = employee.employeeGender === 'M' ? 'Male' : 'Female';

    $(".employee-top-header-name")
        .append(`<span>${employee.firstName} ${employee.lastName}</span>`);

    $(".employee-page-content.employee-page-content-details")
        .append(`<div class="employee-page-info"><p class="employee-info-header">First Name</p><p class="employee-info-content">${employee.firstName}</p></div>`)
        .append(`<div class="employee-page-info"><p class="employee-info-header">Last Name</p><p class="employee-info-content">${employee.lastName}</p></div>`)
        .append(`<div class="employee-page-info"><p class="employee-info-header">Gender</p><p class="employee-info-content">${gender}</p></div>`)
        .append(`<div class="employee-page-info"><p class="employee-info-header">Birth Date</p><p class="employee-info-content">${employee.birthDate.substring(0, 10)}</p></div>`)
        .append(`<div class="employee-page-info"><p class="employee-info-header">Hire Date</p><p class="employee-info-content">${employee.hireDate.substring(0, 10)}</p></div>`);
}

async function loadEmployeeDepartments(uniqueIdentifiers) {
    await tablePlugin({
        ATTACH_SELECTOR: '.employee-page-content.employee-page-content-departments',
        LOADED_ENTITY: ENTITY_TYPES.DEPARTMENT_EMPLOYEE,
        LOADED_ENTITY_IDENTIFIERS: {
            employeeNumber: uniqueIdentifiers.employeeNumber
        }
    });
}

function initEmployeePageListeners() {
    $(".employee-page .employee-page-header .employee-page-top .btn-trash-delete-entity").click(() => {
        
    });
}

export async function loadEmployeePage(uniqueIdentifiers) {
    await apiController.loadHtmlPage(constants.EMPLOYEE_PAGE_RESOURCE);
    await loadEmployeeDetails(uniqueIdentifiers);
    await loadEmployeeDepartments(uniqueIdentifiers);

    navigationMenuItemClickHandler();

    $(`#${constants.EMPLOYEE_PAGE_NAV_DETAILS}`).trigger("click");
}