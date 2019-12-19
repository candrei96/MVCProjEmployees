import ApiController from '../api-controller.js';
import constants from '../utils/constants.js';

const apiController = ApiController.getInstance();

export async function loadDepartmentPage() {
    await apiController.loadHtmlPage(constants.DEPARTMENT_PAGE_RESOURCE);
}