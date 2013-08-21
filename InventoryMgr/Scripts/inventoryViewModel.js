
        $(document).ready(function () {
            //Instantiate our view model and all its methods
            ko.applyBindings(new InventoryItemsModel());
        });

        //Set the visibility property of the addNewFormDiv to visible if the users selects "Add New"
//        $(document).delegate("#btnAddNewInventory", "click", function () {
//            hideAllViews();
//            $("#measurementsContainer").hide();
//            $("#addNewFormDiv").show();
//        });
        //Hide the form when the cancel button clicked
        $(document).delegate("#btnCancelNewInventory, #btnCancelItemEdit, #btnCancelCategoryEdit, #btnCancelCategoryCreate", "click", function () {
            hideAllViews();
            $("#inventoryItemsContainer").show();                
        });

        $(document).delegate("#btnAddNewCategory", "click", function () {
            hideAllViews();
            $("#measurementsContainer").hide();
            $("#addNewCategoryDiv").show();
        });
   
        /********RELATED TO QTY MEASUREMENTS********/  
        $(document).delegate("#btnQtyMeasurements", "click", function(){
            hideAllViews();
            $("#measurementsContainer").show();
        });    
        $(document).delegate("#btnAddNewMeasurement", "click", function(){
            $("#measurementsMain").hide();
            $("#addNewMeasurementDiv").show();
        });
        $(document).delegate("#btnCancelQtyMeasurementCreate", "click", function(){
            $("#addNewMeasurementDiv").hide();
            $("#measurementsMain").show();                
        });
        $(document).delegate("#btnCancelMeasurementEdit", "click", function(){
            $("#editMeasurementDiv").hide();
            $("#measurementsMain").show();                
        });
        $(document).delegate("#btnCloseMeasurements", "click", function(){
            $("#measurementsContainer").hide();
            $("#inventoryItemsContainer").show();
        });

        function hideAllViews(){
            $(".autoHideable").hide();
        }

        //This function will serve as our View Model
        function InventoryItemsModel() {
            var self = this;
            //In this form, we're working with managing Inventory Items, Item Categories, and Item Quantity Measurements
            self.categories = ko.observableArray();
            self.items = ko.observableArray();
            self.measurements = ko.observableArray();
            if (!window.location.origin)
                window.location.origin = window.location.protocol + "//" + window.location.host;
            var uriRoot = window.location.origin;
            var baseUri = uriRoot + "/api/InventoryItems/";
            var categoriesUri = uriRoot + "/api/Categories/";
            var measurementsUri = uriRoot + "/api/Measurements/";

            //First thing we want to do is build a function to (re)populate our models objects
            self.refresh = function () {
                //Clear out all measurements, categories and items
                self.measurements.removeAll();
                self.categories.removeAll();
                self.items.removeAll();
                //Repopulate measurements
                $.getJSON(measurementsUri + currentUserId, function(data){
                    $.each(data, function(){
                        self.measurements.push(this);
                    });
                });
                //Repopulate categories
                $.getJSON(categoriesUri + currentUserId, function(data){
                    $.each(data, function() {
                        self.categories.push(this);
                    });
                });
                //Repopulate items
                $.getJSON(baseUri + currentUserId, function (data) {
                    $.each(data, function () {
                        self.items.push(this);
                    });
                });
            };

            //Now we're going to declare a bunch of functions we'll use to interact with the model

            /******RELATED TO INVENTORY ITEMS************/
            //Function used to pull up a Create form for an new item
            self.getItemCreateForm = function (item) {
                $.get(uriRoot + '/Inventory/Create', function (data) {
                    hideAllViews();
                    $("#addNewFormDiv").html(data).show();
                    $.validator.unobtrusive.parse($("#addNewFormDiv > form"));
                    //$("#inventoryItemsContainer").hide();
                    ko.applyBindings(self, document.getElementById('frmNewItem'));
                });
            };

            self.createItem = function(formElement) {
                if ($(formElement).valid()) {
                    $.post(baseUri, $(formElement).serialize(), null, "json")
                        .done(function(obj) {
                            self.items.push(obj);
                            $("#frmNewItem")[0].reset();
                            //$("#addNewFormDiv").hide();
                            hideAllViews();
                            $("#inventoryItemsContainer").show();
                        });
                }
            };

            self.updateItem = function (item) {
                if($(item).valid()){
                    $.ajax({
                        type: "PUT",
                        url: baseUri, /*+ "Put/"*/
                        data: $(item).serialize(),
                        success: function (data) {
                            //$("#editFormDiv").hide();
                            hideAllViews();
                            $("#inventoryItemsContainer").show();
                            self.refresh();
                        }
                    });
                }
            };
            
            self.removeItem = function (item) {
                $.ajax({ type: "DELETE", url: baseUri + '?Id=' + item.Id })
                    .done(function () { self.items.remove(item); });
            }; 
            //Function used to pull up an edit form for any item
            self.getItemEditForm = function (item) {
                $.get(uriRoot + '/Inventory/Edit/' + item.Id, function (data) {
                    hideAllViews();
                    $("#editFormDiv").html(data).show();
                    $.validator.unobtrusive.parse($("#editFormDiv > form"));
                    //$("#inventoryItemsContainer").hide();
                    ko.applyBindings(self, document.getElementById('frmEditItem'));
                });
            }; 
            /******END INVENTORY ITEM RELATED METHODS******/

            /******RELATED TO CATEGORIES************/
            self.createCategory = function(category){
                if($(category).valid()) {
                    $.post(categoriesUri, $(category).serialize(), null, "json")
                    .done(function (obj) {
                        self.categories.push(obj);
                        $("#frmCreateCategory")[0].reset();
                        //$("#addNewCategoryDiv").hide();
                        hideAllViews();
                        $("#inventoryItemsContainer").show();
                    });
                }
            };
            
            self.updateCategory = function(category){
                if ($(category).valid()) {
                     $.ajax({
                        type: "PUT",
                        url: categoriesUri, /*+ "Put/",*/
                        data: $(category).serialize(),
                        success: function (data) {
                            //$("#editCategoryDiv").hide();
                            hideAllViews();
                            $("#inventoryItemsContainer").show();
                            self.refresh();
                        }
                    });
                }
            };
            self.removeCategory = function(category){
                $.ajax({ type: "DELETE", url: categoriesUri + '?Id=' + category.Id })
                    .done(function () { self.categories.remove(category); });
            };
            self.getCategoryEditForm = function(category){
                $.get(uriRoot + '/Category/Edit/' + category.Id, function (data) {
                    hideAllViews();
                    $("#editCategoryDiv").html(data).show();
                    $.validator.unobtrusive.parse($("#editCategoryDiv > form"));
                    //$("#inventoryItemsContainer").hide();
                    ko.applyBindings(self, document.getElementById('frmEditCategory'));
                });
            }; /******END CATEGORY RELATED METHODS******/

            /******RELATED TO QTY MEASUREMENTS************/
            self.createMeasurement = function(measurement){
                if($(measurement).valid()) {
                    $.post(measurementsUri, $(measurement).serialize(), null, "json")
                    .done(function (obj) {
                        self.measurements.push(obj);
                        $("#frmCreateMeasurement")[0].reset();
                        hideAllViews();
                        $("#measurementsMain").show();
                    });
                }
            };
            self.updateMeasurement = function(measurement){
                if($(measurement).valid()) {
                    $.ajax({
                        type: "PUT",
                        url: measurementsUri,
                        data: $(measurement).serialize(),
                        success: function (data) {
                            //$("#editMeasurementDiv").hide();
                            hideAllViews();
                            $("#measurementsMain").show();
                            self.refresh();
                        }
                    });
                }
            };
            self.removeMeasurement = function(measurement){
                $.ajax({ type: "DELETE", url: measurementsUri + '?Id=' + measurement.Id })
                    .done(function () { self.measurements.remove(measurement); });
            };
            self.getMeasurementEditForm = function(measurement){
                $.get(uriRoot + '/Measurement/Edit/' + measurement.Id, function (data) {
                    $("#measurementsMain").hide();
                    $("#editMeasurementDiv").html(data).show();                    
                    $.validator.unobtrusive.parse($("#editMeasurementDiv > form"));
                    ko.applyBindings(self, document.getElementById('frmEditMeasurement'));
                });
            }; /******END QTY MEASUREMENT RELATED METHODS******/

            /******FILTERING AND OTHER FUNCTIONS******/
            self.itemsInCategory = function(category){
                return ko.utils.arrayFilter(self.items(), function(item){
                    return item.InventoryCategory.Id === category.Id;
                });
            };
            self.itemsUsingMeasurement = function(measurement){
                return ko.utils.arrayFilter(self.items(), function(item){
                    return item.InventoryQtyMeasurements.Id === measurement.Id;
                });
            };

            //Make this knockout observable so that the UI will automatically respond to this changing.
            self.showEmptyCategories = ko.observable(false);

            //After creating all of our model behaviours, tell it to refresh its items
            self.refresh();

        }