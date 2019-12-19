// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
import {
    initUnloadedPageLogic
} from './pages/main-page-logic.js';

import constants from './utils/constants.js';
import { ENTITY_TYPES } from './utils/enums.js';

window.onunload = function () {
    window.sessionStorage.clear();
}

function main() {
    initUnloadedPageLogic();

    window.sessionStorage.setItem(constants.STORAGE_PAGE_COUNTER, 0);
    window.sessionStorage.setItem(constants.STORAGE_SELECTED_ENTITY_KEY, ENTITY_TYPES.EMPLOYEE);
    $(`#${constants.EMPLOYEE_TAB_ID}`).trigger("click");
};

main();
