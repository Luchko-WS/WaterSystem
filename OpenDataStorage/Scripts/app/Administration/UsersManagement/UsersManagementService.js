(function () {
    'use strict';

    angular
        .module('MainApp')
        .factory('UsersManagementService', UsersManagementService);

    UsersManagementService.$inject = ['$http'];

    function UsersManagementService($http) {
        var service = {
            getUser: getUser,
            getCurrentUser: getCurrentUser,
            getAllRoles: getAllRoles,
            addUserToRole: addUserToRole,
            removeUserFromRole: removeUserFromRole,
            getUsersList: getUsersList,
            setLockState: setLockState,
            changePassword: changePassword,
            deleteUser: deleteUser
        };

        return service;

        function getUser(userName) {
            return $http.get("/api/UsersManagement/" + userName);
        }

        function getCurrentUser() {
            return $http.get("/api/UsersManagement/GetCurrentUser");
        }

        function getAllRoles() {
            return $http.get("/api/UsersManagement/GetAllRoles");
        }

        function addUserToRole(username, roleName) {
            return $http.post("/api/UsersManagement/AddUserToRole/" + username + "/" + roleName);
        }

        function removeUserFromRole(username, roleName) {
            return $http.post("/api/UsersManagement/RemoveUserFromRole/" + username + "/" + roleName);
        }

        function getUsersList() {
            return $http.get("/api/UsersManagement/GetUsersList");
        }

        function setLockState(userName, lockState) {
            return $http.post("/api/UsersManagement/SetLockState/" + userName + "/" + lockState);
        }
        
        function deleteUser(id) {
            return $http.delete("/api/UsersManagement/Delete/" + id);
        }

        function changePassword(userName, newPassword) {
            return $http.post("/api/UsersManagement/ChangePasswordForUser/" + userName + "/" + newPassword);
        }
    }
})();