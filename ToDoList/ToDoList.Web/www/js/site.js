/*
sample site.js file controlling the angular application

not very clean, especially all in one file. forgive me.

*/

console.log("starting angular app");
var todos = angular.module("todos", []);

todos.controller("ToDoListController", ToDoListController);

function ToDoListController($http) {
    var self = this;

    console.log("starting ToDoListController");

    // ToDo Liste abrufen
    self.fetch = function() {
        console.log("loading ToDos");

        $http.get("/api/todo", { cache: false })
             .then(
                 function(response) { self.todos = response.data; },
                 function(response) { throw new Error("load error"); }
             );
    };

    // ToDo speichern
    self.specify = function() {
        console.log("specifying ToDo");

        $http.post("/api/todo", self.todo);
    };

    // Daten ToDo Liste
    self.todos = [];

    // Daten Eingabeformular
    self.todo = {
        id: "einkaufen",
        description: "Unbedingt Milch einkaufen"
    };

    self.fetch();
}
