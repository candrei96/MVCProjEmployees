import { extendOptions } from '../utils/utilityKit.js';
import { ENTITY_TYPES } from '../utils/enums.js';

let modalPlugin = (function () {
    let instanceNumber = 0;

    function addModalListeners() {
        $(".close").click(() => {
            $(".modal").hide();
        });

        $(`#create-modal-plugin-modal${context.options.IDENTITY_NUMBER} .btn-submit-entity`).click(() => {
            switch (context.options.LOADED_ENTITY) {
                case ENTITY_TYPES.DEPARTMENT_EMPLOYEE:

                    break;
                case ENTITY_TYPES.TITLE:

                    break;
                case ENTITY_TYPES.SALARY:

                    break;
                default:
                    throw new Error('Invalid entity type');
                    break;
            }
        });

        $(`#delete-modal-plugin-modal${context.options.IDENTITY_NUMBER} .btn-delete-entity`).click(() => {
            switch (context.options.LOADED_ENTITY) {
                case ENTITY_TYPES.DEPARTMENT_EMPLOYEE:

                    break;
                case ENTITY_TYPES.TITLE:

                    break;
                case ENTITY_TYPES.SALARY:

                    break;
                default:
                    throw new Error('Invalid entity type');
                    break;
            }
        });
    }

    function addCreateModal(context) {
        let createModalContent = '';

        switch (context.options.LOADED_ENTITY) {
            case ENTITY_TYPES.DEPARTMENT_EMPLOYEE:
                createModalContent += `
                        <div class="modal-header">
                            <h3>Add Employee Department</h3>
                            <span class="close">&times;</span>
                        </div>
                        <div class="modal-row">
                            <p>Department Number</p>
                            <input class="modal-input modal-input-departmentEmployee-departmentNumber form-control" type="text" />
                        </div>
                `;
                break;
            case ENTITY_TYPES.TITLE:
                createModalContent += `
                        <div class="modal-header">
                            <h3>Add Employee Title</h3>
                            <span class="close">&times;</span>
                        </div>
                        <div class="modal-row">
                            <p>Title</p>
                            <input class="modal-input modal-input-title-employeeTitle form-control" type="text" />
                        </div>
                        <div class="modal-row">
                            <p>Start Date</p>
                            <input class="modal-input modal-input-title-from-date form-control datepicker" />
                        </div>
                        <div class="modal-row">
                            <p>To Date</p>
                            <input class="modal-input modal-input-title-to-date form-control datepicker" />
                        </div>
                `;
                break;
            case ENTITY_TYPES.SALARY:
                createModalContent += `
                        <div class="modal-header">
                            <h3>Add Employee Salary</h3>
                            <span class="close">&times;</span>
                        </div>
                        <div class="modal-row">
                            <p>Salary Amount</p>
                            <input class="modal-input modal-input-salary-employeeSalary form-control" type="text" />
                        </div>
                        <div class="modal-row">
                            <p>Start Date</p>
                            <input class="modal-input modal-input-salary-from-date form-control datepicker" />
                        </div>
                        <div class="modal-row">
                            <p>To Date</p>
                            <input class="modal-input modal-input-salary-to-date form-control datepicker" />
                        </div>
                `;
                break;
            default:
                throw new Error('Invalid entity type');
                break;
        }
        const createModalHtml = `
                <div id="create-modal-plugin-modal${context.options.IDENTITY_NUMBER}" class="modal modal-create">
                    <div class="modal-content">
                        ${createModalContent}
                        <div class="modal-footer">
                            <div class="modal-error-zone"></div>
                            <button class="btn btn-primary btn-submit-entity"><span>Add</span></button>
                        </div>
                    </div>
                </div>`;

        $(context.options.ATTACH_SELECTOR)
            .prepend(createModalHtml);
    }


    function addDeleteModal(context) {
        let modalContent = '';
        switch (context.options.LOADED_ENTITY) {
            case ENTITY_TYPES.DEPARTMENT_EMPLOYEE:
                modalContent += `
                    <div class="modal-header">
                        <h3>Delete Employee Department</h3>
                        <span class="close">&times;</span>
                    </div>
                    <div class="modal-row">
                        <p>You are about to <strong>DELETE</strong> the employee's department. This action cannot be undone and the data will be lost.</p>
                    </div>
                `;
                break;
            case ENTITY_TYPES.TITLE:
                modalContent += `
                    <div class="modal-header">
                        <h3>Delete Employee Title</h3>
                        <span class="close">&times;</span>
                    </div>
                    <div class="modal-row">
                        <p>You are about to <strong>DELETE</strong> the employee's title. This action cannot be undone and the data will be lost.</p>
                `;
                break;
            case ENTITY_TYPES.SALARY:
                modalContent += `
                    <div class="modal-header">
                        <h3>Delete Employee Salary</h3>
                        <span class="close">&times;</span>
                    </div>
                    <div class="modal-row">
                        <p>You are about to <strong>DELETE</strong> the employee's salary. This action cannot be undone and the data will be lost.</p>
                    </div>
                `;
                break;
            default:
                throw new Error('Invalid entity type');
                break;
        }

        const deleteModalHtml = `
            <div id="delete-modal-plugin-modal${context.options.IDENTITY_NUMBER}" class="modal modal-delete">
                <div class="modal-content">
                    ${modalContent}
                    <div class="modal-row">
                        <ul class="modal-deleted-entity-list">
                        </ul>
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-primary btn-delete-entity"><span>Delete</span></button>
                    </div>
                </div>
            </div>  
        `;

        $(context.options.ATTACH_SELECTOR)
            .prepend(deleteModalHtml);
    }

    function createModal(context) {
        addCreateModal(context);
        addDeleteModal(context);
        addModalListeners();
    }

    function ModalPlugin(options) {
        this.defaultOptions = {
            ATTACH_SELECTOR: '',
            LOADED_ENTITY: -1,
            LOADED_ENTITY_IDENTIFIERS: {},
            IDENTITY_NUMBER: 0
        };

        this.options = extendOptions(options, this.defaultOptions);
    }

    function initPlugin(options) {
        instanceNumber = instanceNumber + 1;
        options.IDENTITY_NUMBER = instanceNumber;
        const modalPlugin = new ModalPlugin(options);

        createModal(modalPlugin);
    }

    return initPlugin;
})();

export default modalPlugin;