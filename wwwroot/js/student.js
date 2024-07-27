/// <reference path="knockout.js" />
function Employee(data) {
    var self = this;
    //self.EmployeeId = ko.observable(data.employeeId);
    self.EmployeeId = ko.observable(data.id);
    self.EmpName = ko.observable(data.name);
    self.DOB = ko.observable(data.dob);
    //self.wantsSpam = ko.observable(true);
    self.Gender = ko.observable(data.gender || "Male");
    self.Salary = ko.observable(data.salary);
    self.EntryBy = self.EmpName;
    self.EntryDate = new Date().toISOString().slice(0, 10);
    self.empQualifications = ko.observableArray(data.empQualifications || []);
}

function EmpQualification(data) {
    var self = this;
    self.EmployeeId = ko.observable(data.employeeId);

    self.qId = ko.observable(data.qId || '');
    //self.qName = ko.observable(data.qName);
    //self.qId = ko.observable(data.id || '');
    self.id = ko.observable(data.id || '');
    self.qName = ko.observable(data.name);
    self.marks = ko.observable(data.marks);

}


function EmployeeViewModel() {
    var self = this;

    //self.mode = ko.observable('a');
    self.Employee = ko.observable(new Employee({}));
    self.empQual = ko.observable(new EmpQualification({}));
    self.ArrayQualification = ko.observableArray([]);
    self.QualificationLists = ko.observableArray([]);
    self.Employees = ko.observableArray([]);


    //fetched the  Qualification List
    self.fetchQualification = function () {
        $.ajax({
            url: '/api/Employee/GetQualification',
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                var mappedQualifications = $.map(data.$values, function (item) {
                    return new EmpQualification(item);
                });
                self.QualificationLists(mappedQualifications);

            },
            error: function (err) {
                alert("Error fetching courses: " + err.status + " - " + err.statusText);
            }
        });
    };
    self.fetchQualification();
    
    //Add Qualification

    self.addqualification = function () {
        self.Employee().empQualifications.push(ko.toJS(self.empQual));
        self.empQual(new EmpQualification({}));

    }


    //Delete Qualification

    self.deleteQualification = function (data) {
        self.Employee().empQualifications.remove(data);
        alert("Qualification deleted!!!");
    }

    // Add data to DB
    self.AddEmployee = function () {
        var employeeDto = {
            Name: self.Employee().EmpName(),
            DOB: self.Employee().DOB(),
            Gender: self.Employee().Gender(),
            Salary: self.Employee().Salary(),
            Qualifications: ko.toJS(self.Employee().empQualifications).map(function (qual) {
                return {
                    QualificationId: qual.id,
                    Marks: qual.marks
                };
            })
        };

        $.ajax({
            url: '/api/Employee/add',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(employeeDto),
            success: function (data) {
                self.GetallEmployee();
                self.Employee(new Employee({}));
                alert('Employee Added Successfully!');
            },
            error: function (err) {
                alert("Error adding Employee: " + err.status + " - " + err.statusText);
            }
        });
    };
    // Get All Employee
    self.GetallEmployee = function () {
        $.ajax({
            type: "GET",
            url: '/api/Employee/GetEmployees',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: {},
            success: function (data) {
                var mappedEmployees = $.map(data.$values, function (item) {
                    return new Employee(item);
                });
                self.Employees(mappedEmployees);

            },
            error: function (err) {
                console.error("Error fetching employees:", err);
                alert("Error fetching employees.");
            }
        });
    }
    self.GetallEmployee();

    //Delete the Employee
    self.deleteEmployee = function (employee) {
        var id = employee.EmployeeId();
        $.ajax({
            type: "DELETE",
            url: "/api/Employee/" + id,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                self.GetallEmployee();
                alert('Employee Deleted Successfully!');
            },
            error: function (error) {
                alert(error.status + " - " + error.statusText);
            }
        });
    }

}
ko.applyBindings(new EmployeeViewModel());




