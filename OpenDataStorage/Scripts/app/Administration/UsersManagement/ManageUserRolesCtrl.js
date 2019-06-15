(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('ManageUserRolesCtrl', ManageUserRolesCtrl);

    ManageUserRolesCtrl.$inject = ['$uibModal', 'UserService', 'MessageService', '$filter'];

    function ManageUserRolesCtrl($uibModal, UserService, MessageService, $filter) {
        /* jshint validthis:true */
        var vm = this;

        vm.users = [];
        
        vm.removeUserFromSelectedRole = removeUserFromSelectedRole;
        vm.addNewUserToRole = addNewUserToRole;
        vm.isUserDefault = isUserDefault;

        vm.filterValues = {};
        vm.toggleFilter = toggleFilter;
        vm.applyFilter = applyFilter;
        vm.changeSortingOrder = changeSortingOrder;
        vm.showSortingDirection = showSortingDirection;
        vm.filterShow = false;

        vm.onUserRoleChanged = onUserRoleChanged;
        vm.loadAllRoles = function() { return UserService.getAllRoles(); };

        vm.pageChanged = pageChanged;
        vm.pageOptions = {
            usersPerPage: 30,
            currentPage: 1
        };

        activate();

        function activate() {
            vm.orderBy = {
                field: "UserName",
                ascOrder: true
            };

            UserService.getAllRoles().then(function(res) {
                vm.allRoles = res.data;
            });
        }

        function onUserRoleChanged() {
            if (vm.selectedRole) {
                UserService.getPagedUsersInRole(vm.filterValues, vm.orderBy, vm.selectedRole, 0, vm.pageOptions.usersPerPage).then(function(res) {
                    vm.users = res.data.pagedUsers;
                    vm.pageOptions.totalUsers = res.data.totalCount;
                });
            }
        }

        function isUserDefault(user) {
            var role = $filter('filter')(user.defaultInRoles, vm.selectedRole)[0];
            if (role) {
                return 'Yes';
            }

            return 'No';
        }

        function addNewUserToRole() {
            var modalInstance = $uibModal.open({
                templateUrl: '/Account/AddNewUserToRole/',
                controller: 'AddUserToRoleCtrl',
                controllerAs: 'vm',
                backdrop: 'static',
                resolve: {
                    mrOptions: {
                        roleId: vm.selectedRole,
                        roleName: $filter('filter')(vm.allRoles, { value: vm.selectedRole })[0].text
                    }
                }
            });

            modalInstance.result.then(function(res) {
                if (res) {
                    onUserRoleChanged();
                }
            });
        }

        function removeUserFromSelectedRole(user) {
            MessageService.showMessageYesNo("ng_Remove_User_From_Role_ConfirmMessage", "ng_Remove_User_From_Role", { userName: user.userName, userRole: $filter('filter')(vm.allRoles, { value: vm.selectedRole })[0].text}).then(function (result) {
                if (result === "OK") {
                    UserService.removeUserFromRole(user.userName, vm.selectedRole).then(function (response) {
                        if (response.status === 200) {
                            var index = vm.users.indexOf(user);
                            if (index > -1) {
                                vm.users.splice(index, 1);
                            }
                            vm.pageOptions.totalUsers--;
                        } else {
                            MessageService.showMessage("ng_Error_Try_Latter", "ng_Error_Title");
                        }
                    }, function (error) {
                        MessageService.showMessage("ng_Error_Try_Latter", "ng_Error_Title");
                    });
                }
            });
        }

        function changeSortingOrder(fieldName) {
            if (vm.orderBy.field !== fieldName) {
                vm.orderBy.field = fieldName;
                vm.orderBy.ascOrder = true;
            } else {
                vm.orderBy.ascOrder = !vm.orderBy.ascOrder;
            }
            pageChanged();
        }

        function showSortingDirection(fieldName, ascCtrl) {
            if (vm.orderBy.field !== fieldName) {
                return false;
            }
            if (vm.orderBy.ascOrder && ascCtrl) {
                return true;
            } else if (!vm.orderBy.ascOrder && !ascCtrl) {
                return true;
            } else {
                return false;
            }
        }

        function applyFilter() {
            if (vm.selectedRole) {
                UserService.getPagedUsersInRole(vm.filterValues, vm.orderBy, vm.selectedRole, 0, vm.pageOptions.usersPerPage).then(function (res) {
                    vm.users = res.data.pagedUsers;
                    vm.pageOptions.totalUsers = res.data.totalCount;
                });
            }
        }

        function pageChanged() {

            if (vm.selectedRole) {
                UserService.getPagedUsersInRole(vm.filterValues, vm.orderBy, vm.selectedRole, (vm.pageOptions.currentPage - 1) * vm.pageOptions.usersPerPage, vm.pageOptions.usersPerPage).then(function (res) {
                    vm.users = res.data.pagedUsers;
                });
            }
        }

        function toggleFilter() {
            vm.filterShow = !vm.filterShow;
            if (!vm.filterShow) {
                vm.filterValues = {};
                applyFilter();
            }
        }
    }
})();