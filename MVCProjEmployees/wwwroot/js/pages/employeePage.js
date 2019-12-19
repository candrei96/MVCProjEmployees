import ApiController from '../api-controller.js';
import constants from '../utils/constants.js';

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

function initEmployeePageListeners() {
    $(".employee-page .employee-page-header .employee-page-top .btn-trash-delete-entity").click(() => {
        
    });
}

export async function loadEmployeePage() {
    await apiController.loadHtmlPage(constants.EMPLOYEE_PAGE_RESOURCE);

    navigationMenuItemClickHandler();

    $(`#${constants.EMPLOYEE_PAGE_NAV_DETAILS}`).trigger("click");
}