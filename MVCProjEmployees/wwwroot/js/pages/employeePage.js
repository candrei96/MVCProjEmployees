import ApiController from '../api-controller.js';
import constants from '../utils/constants.js';

const apiController = ApiController.getInstance();

export async function loadEmployeePage() {
    await apiController.loadHtmlPage(constants.EMPLOYEE_PAGE_RESOURCE);
}