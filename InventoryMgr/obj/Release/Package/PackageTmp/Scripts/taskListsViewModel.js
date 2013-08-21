///<reference path="../Scripts/jquery-1.7.1-vsdoc.js"/>
///<reference path="../Scripts/knockout-1.1.0.js"/>

        $(document).ready(function(){
            //Instantiate our view model and all its methods
            ko.applyBindings(new TaskListsModel());
        });

        $(document).delegate("#btnAddNewTaskList", "click", function(){
            hideAllViews();
            $("#divAddNewTaskList").show();
        });
        
        //Any buttons that need to hide all views and then display the main one again
        $(document).delegate("#btnCancelEditTaskList, #btnCloseDetails, #btnCancelCreateTaskList", "click", function(){
            hideAllViews();
            $("#tasklistsContainer").show();
        });

        //Any buttons that need to hide all views and then display the TaskListDeatils one again
        $(document).delegate("#btnCancelCreateGeneralTask, #btnCancelCreateInventoryTask", "click", function () {
            ShowTaskListDetails();
        });

        function hideAllViews(){
            $(".autoHideable").hide();
        }

        function ShowTaskListDetails() {
            hideAllViews();
            $("#divTaskListDetails").show();
        }

        function toggleModalDetails(){
            var modalDiv = document.getElementById("divModalDetails");
            
            //Only if our modal has content we'll go ahead and clear the content so to manage DOM size.
            if (modalDiv.style.visibility == "visible") {
                $(modalDiv).html("");
            }

            modalDiv.style.visibility = (modalDiv.style.visibility == "visible") ? "hidden" : "visible";
        }


        //Create ViewModel with Knockout
        function TaskListsModel(){
            var self = this;
            //In this module, we're working with TaskLists, TaskItems and InventoryTaskItems
            //Important!! TaskItems and InventoryTaskItems are contained within TaskLists, so the observableArray that serves as our view model will only be of the TaskLists.
            self.taskLists = ko.observableArray();
            
            //URLs for our web service calls
            if (!window.location.origin)
                window.location.origin = window.location.protocol + "//" + window.location.host;
            var uriRoot = window.location.origin;
            var taskListsUri = uriRoot + "/api/TaskLists/", tasksUri = uriRoot + "/api/GeneralTasks/", inventoryTasksUri = uriRoot + "/api/InventoryTasks/";

            //Observable to be used when getting details for a specific list
            self.focusedList = ko.observable();
            self.focusedList.Tasks = ko.observableArray();

            //Observable object to be used for displaying Task Details and Edit partial views in a modal
            self.focusedTask = ko.observable();


            //First thing we want to do is (re)populate our models objects
            self.refresh = function(){
                //First, remove all items from our array
                self.taskLists.removeAll();
                //Repopulate TaskLists
                $.getJSON(taskListsUri + currentUserId, function(data){
                    $.each(data, function(){
                        self.taskLists.push(this);
                        if(self.focusedList != null) {
                            if(this.Id === self.focusedList.Id){
                                self.focusedList = this;
                                //If the display style has a value at all, it won't be hidden.
                                if($("#divTaskListDetails")[0].style.display && $("#divTaskListDetails")[0].style.display !== "none"){
                                    self.getTaskListDetails(this);
                                }
                            }
                        }
                    });
                });
            }; //RELATED TO TASK LISTS
            self.createTaskList = function(formElement){
                if($(formElement).valid()) {
                    $.post(taskListsUri, $(formElement).serialize(), null, "json")
                    .done(function(obj){
                        self.taskLists.push(obj);
                        $("#frmCreateTaskList")[0].reset();
                        hideAllViews();
                        $("#tasklistsContainer").show();
                    });
                }
            };
            self.getTaskListDetails = function (tl) {
                self.focusedList = tl;
                hideAllViews();
                //Load the template from an external .htm file
                $.get((uriRoot + '/ClientSideTemplates/taskListDetailsTemplate.htm'), function (data) {
                    $("#divTaskListDetails").html(data).show();
                    ko.applyBindings(self, $(".taskListDetails")[0]); 
                });                
            };
            self.getTaskListEditForm = function(tl){
                $.get(uriRoot + '/TaskList/Edit/' + tl.Id, function(data){
                    hideAllViews();
                    $("#divEditTaskList").html(data).show();
                    $.validator.unobtrusive.parse($("#divEditTaskList > form"));
                    ko.applyBindings(self, document.getElementById('frmEditTaskList'));
                });
            };
            self.changeTaskListStatus = function(tl){
                $.get(uriRoot + '/TaskList/Edit/' + tl.Id, function(data){
                    $("#divEditTaskList").html(data);
                    //Now, we're going to modify the IsComplete value
                    var IsComplete = $("#frmEditTaskList input[id='IsComplete']").not('[type="hidden"]')[0];
                    var currentlyChecked = IsComplete.checked;
                    $(IsComplete).attr("checked", !currentlyChecked);
                    self.updateTaskList($("#frmEditTaskList")[0]);
                });                
            };
            self.updateTaskList = function(tl){
                if($(tl).valid()) {
                    //Due to how MVC auto-adds a hidden field for 'false' as un-checked checkboxes don't post,
                    //we'll remove the hidden field if the checkbox is checked, and leave otherwise.
                    if ($("#frmEditTaskList input[name='IsComplete']:checked").length > 0) {
                        $($("#frmEditTaskList input[name='IsComplete']")[1]).remove();//Go ahead and remove the hidden one
                        tl = $("#frmEditTaskList");
                    }
                    $.ajax({
                        type: "PUT",
                        url: taskListsUri,
                        data: $(tl).serialize(),
                        success: function(data){
                            //Go ahead and clear the contents of the div so that it's not taking up resources.
                            $("#divEditTaskList").html("");
                            hideAllViews();    
                            $("#tasklistsContainer").show();
                            self.refresh();                        
                        }
                    });
                }
            };
            self.deleteTaskList = function(tl){
                $.ajax({ type: "DELETE", url: taskListsUri + tl.Id })
                    .done(function () { self.taskLists.remove(tl); });
            }; 
            
            //RELATED TO INVENTORY TASKS
            self.getNewInventoryTaskForm = function(tl){
                //Accepts the task list we're calling from, so we can get the ID to associate the new task with this list
                $.get(uriRoot + "/InventoryTask/Create/" + tl.Id, function(data){
                    hideAllViews();
                    $("#divAddNewTask").html(data).show();
                    $.validator.unobtrusive.parse($("#divAddNewTask form"));
                    ko.applyBindings(self, document.getElementById('divAddNewTask'));
                });
            };
            self.createInventoryTask = function(formElement){
                if ($(formElement).valid()) {
                    $.post(inventoryTasksUri, $(formElement).serialize(), null, "json")
                    .done(function (obj) {
                        $("#divAddNewTask").html('');
                        hideAllViews();
                        $("#divTaskListDetails").show();
                        self.refresh();
                    });
                }
            };
            self.getInventoryTaskEditForm = function(iTask){
                $.get(uriRoot + "/InventoryTask/Edit/" + iTask.Id, function(data){
                    self.focusedTask = iTask;
                    $("#divModalDetails").html(data);                    
                    ko.applyBindings(self, document.getElementById("divModalDetails"));
                    $.validator.unobtrusive.parse($("#divModalDetails form")[0]);
                });
            };
            self.getInventoryTaskDetailsForm = function (iTask) {
                $.get(uriRoot + "/InventoryTask/Details/" + iTask.Id, function (data) {
                    self.focusedTask = iTask;
                    hideAllViews();
                    $("#divModalDetails").html(data).show();                    
                    //toggleModalDetails();
                    ko.applyBindings(self, document.getElementById("divModalDetails"));
                });
            };
            self.updateInventoryTask = function(iTask){
                if($(iTask).valid()) {

                    //Due to how MVC auto-adds a hidden field for 'false' as un-checked checkboxes don't post,
                    //we'll remove the hidden field if the checkbox is checked, and leave otherwise.
                    if ($("#frmEditInventoryTask input[name='Completed']:checked").length > 0) {
                        $($("#frmEditInventoryTask input[name='Completed']")[1]).remove();//Go ahead and remove the hidden one
                        iTask = $("#frmEditInventoryTask");
                    }

                    $.ajax({
                        type: "PUT",
                        url: inventoryTasksUri,
                        data: $(iTask).serialize(),
                        success: function (data) {
                            if (document.getElementById("divModalDetails").style.visibility == "visible") {
                                toggleModalDetails();
                            }
                            self.refresh();
                        }
                    });
                }
            };
            self.changeInventoryTaskStatus = function(iTask){
                $.get(uriRoot + "/InventoryTask/Edit/" + iTask.Id, function(data){
                    $("#divEditTask").html(data);
                    //Now, we're going to modify the IsComplete value
                    var IsComplete = $("#frmEditInventoryTask input[name='Completed']").not("[type='hidden']")[0];
                    var currentlyChecked = IsComplete.checked;
                    $(IsComplete).attr("checked", !currentlyChecked);
                    self.updateInventoryTask($("#frmEditInventoryTask")[0]);
                });
            };
            self.deleteInventoryTask = function(iTask){
                $.ajax({ type: "DELETE", url: inventoryTasksUri + "?Id=" + iTask.Id })
                    .done(function () { self.refresh(); });
            }; //RELATED TO GENERAL TASKS
            self.getNewGeneralTaskForm = function(tl){
                //Accepts the task list we're calling from, so we can get the ID to associate the new task with this list
                $.get(uriRoot + "/GeneralTask/Create/" + tl.Id, function(data){
                    hideAllViews();
                    $("#divAddNewTask").html(data).show();
                    $.validator.unobtrusive.parse($("#divAddNewTask form"));
                    ko.applyBindings(self, document.getElementById('divAddNewTask'));
                });
            };
            self.createGeneralTask = function(formElement){
                if ($(formElement).valid()) {
                    $.post(tasksUri, $(formElement).serialize(), null, "json")
                    .done(function (obj) {
                        $("#divAddNewTask").html('');
                        hideAllViews();
                        $("#divTaskListDetails").show();
                        self.refresh();
                    });
                }
            };
            self.getGeneralTaskEditForm = function(gTask){
                $.get(uriRoot + "/GeneralTask/Edit/" + gTask.Id, function(data){
                    self.focusedTask = gTask;
                    $("#divModalDetails").html(data);                    
                    ko.applyBindings(self, document.getElementById("divModalDetails"));
                    $.validator.unobtrusive.parse($("#divModalDetails form")[0]);
                });
            };
            self.getGeneralTaskDetailsForm = function (gTask) {
                $.get(uriRoot + "/GeneralTask/Details/" + gTask.Id, function (data) {
                    self.focusedTask = gTask;
                    hideAllViews();
                    $("#divModalDetails").html(data).show();
                    //toggleModalDetails();
                    ko.applyBindings(self, document.getElementById("divModalDetails"));
                });
            };
            self.updateGeneralTask = function(gTask){
                if($(gTask).valid()) {

                    //Due to how MVC auto-adds a hidden field for 'false' as un-checked checkboxes don't post,
                    //we'll remove the hidden field if the checkbox is checked, and leave otherwise.
                    if ($("#frmEditGeneralTask input[name='Completed']:checked").length > 0) {
                        $($("#frmEditGeneralTask input[name='Completed']")[1]).remove();//Go ahead and remove the hidden one
                        gTask = $("#frmEditGeneralTask");
                    }

                    $.ajax({
                        type: "PUT",
                        url: tasksUri,
                        data: $(gTask).serialize(),
                        success: function (data) {
                            if (document.getElementById("divModalDetails").style.visibility == "visible") {
                                toggleModalDetails();
                            }
                            self.refresh();
                        }
                    });
                }
            };
            self.changeGeneralTaskStatus = function(gTask){
                $.get(uriRoot + "/GeneralTask/Edit/" + gTask.Id, function(data){
                    $("#divEditTask").html(data);
                    //Now, we're going to modify the Completed value
                    var IsComplete = $("#frmEditGeneralTask input[name='Completed']").not("[type='hidden']")[0];
                    var currentlyChecked = IsComplete.checked;
                    $(IsComplete).attr("checked", !currentlyChecked);
                    self.updateGeneralTask($("#frmEditGeneralTask")[0]);
                });
            };
            self.deleteGeneralTask = function(gTask){
                $.ajax({ type: "DELETE", url: tasksUri + "?Id=" + gTask.Id })
                    .done(function () { self.refresh(); });
            }; 
            
            //Filtering and other functions
            
            self.incompleteTaskLists = function(){
                return ko.utils.arrayFilter(self.taskLists(), function(tl){
                    return tl.IsComplete == false;
                });
            };
            
            self.inventoryTasks = function(){                
                return ko.utils.arrayFilter(self.focusedList.Tasks, function(t){
                    return t.InventoryItemId && 
                    (self.showCompletedTasks() || !t.Completed);
                });
            };
            
            self.incompleteInventoryTasks = function () {
                return ko.utils.arrayFilter(self.focusedList.Tasks, function (t) {
                    return t.InventoryItemId != null && !t.Completed;
                });
            };
            
            self.generalTasks = function(){
                return ko.utils.arrayFilter(self.focusedList.Tasks, function(t){
                    return !t.InventoryItemId &&
                    (self.showCompletedTasks() || !t.Completed);
                });
            };
            
            self.incompleteGeneralTasks = function () {
                return ko.utils.arrayFilter(self.focusedList.Tasks, function (t) {
                    return !t.InventoryItemId && !t.Completed;
                });
            }; 
            
            //Make these knockout observables so that the UI will automatically respond to this changing.
            self.showCompletedTaskLists = ko.observable(false);
            self.showCompletedTasks = ko.observable(false);

            self.getListTasks = function (l) {
                return ko.utils.arrayFilter(self.taskLists(), function (tl) {
                    return tl.Id == l.Id;
                });
            }; //After creating all of our model behaviours, tell it to refresh its items
            self.refresh();
        }