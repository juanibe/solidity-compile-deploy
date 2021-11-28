// SPDX-License-Identifier: MIT
pragma solidity ^0.8.3;

/**
 * This contract will have an array of users, and they will be able to register
 * using their ethereum account
 */
contract UsersContract {
    struct User {
        string name;
        string surname;
    }
    
    mapping(address => User) private users;
    mapping(address => bool) private joinedUsers;
    address[] total;

    event onUserJoined(address, string);

    /* With this users will join our users mapping */
    function join(string memory name, string memory surname) public {
        require(!userJoined(msg.sender));
        User storage user = users[msg.sender];
        user.name = name;
        user.surname = surname;
        joinedUsers[msg.sender] = true;
        total.push(msg.sender);

        emit onUserJoined(msg.sender, string(abi.encodePacked(name, " ", surname)));
    } 

    /**
     * Return a tuple 
     */
    function getUser(address addr) public view returns (string memory name, string memory surname) {
        require(userJoined(msg.sender));
        /* We are only consulting data so we use memory */
        User memory user = users[addr];
        return (user.name, user.surname);
    }

    function userJoined(address addr) private view returns (bool) {
        return joinedUsers[addr];
    }

    function totalUsers() public view returns (uint){
        return total.length;
    }
}
