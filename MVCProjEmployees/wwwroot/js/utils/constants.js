let constantObject;

constantObject = {
    API_URL: "https://localhost:44328",
    EMPLOYEE_TAB_RESOURCE: "empltab",
    EMPLOYEE_PAGE_RESOURCE: "employeePage",
    EMPLOYEE_TAB_ID: "employees-tab",
    DEPARTMENT_TAB_RESOURCE: "departmenttab",
    DEPARTMENT_PAGE_RESOURCE: "departmentPage",
    DEPARTMENT_TAB_ID: "departments-tab",
    SALARY_TAB_RESOURCE: "salarytab",
    SALARY_TAB_ID: "salaries-tab",
    MAXIMUM_TABLE_ROWS: 15,
    DATABASE_DEFAULT_DATE: new Date(9999, 0, 1),
    DATABASE_STRING_DEFAULT_DATE: '9999-01-01',
    STORAGE_SELECTED_ENTITY_KEY: "selected-entity-key",
    STORAGE_PAGE_COUNTER: "storage-page-counter-key",
    CURRENT_APPLIED_FILTER: "storage-current-applied-filter-key",
    CURRENT_APPLIED_SORT_ORDER: "storage-current-applied-sort-order-key",
    LAST_QUERY_STATE: "storage-last-query-type"
};

Object.freeze(constantObject);

export default constantObject;