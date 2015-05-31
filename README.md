![EstimatorX](https://raw.githubusercontent.com/loresoft/Estimatorx/master/Code/Source/Estimatorx.Web/Content/EstimatorX-logo.png)

A simple project estimation application.   
[http://estimatorx.com](http://estimatorx.com "EstimatorX")

[![Build status](https://ci.appveyor.com/api/projects/status/8pc9g61vm9yxqrbc)](https://ci.appveyor.com/project/LoreSoft/estimatorx)

## Features

[![Join the chat at https://gitter.im/loresoft/Estimatorx](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/loresoft/Estimatorx?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

### Projects

A project contains all the details that make up an estimate. An estimate is broken down into Assumptions, Factors and Tasks. The estimate is padded with a contingency rate.

### Assumptions

When making an estimate, there are assumptions the estimator makes to come up with the estimate. Document those assumptions to help raise the red flag in the future when an assumptions proves not to be true.

### Factors

Factors are a type of task with hours associated with the complexity of that task. Factors allow the estimator to state hours based on a specific type of task, regardless of the project.

### Tasks

Tasks are a specific item or feature of the project being estimated. A task is assigned a Factor. The estimator enters the number of tasks per complexity level.

Tasks are part of a Section. Sections are a way to group a set of common features. Tasks totals are also rolled up to the section level.

### Reports

Project estimates can be displayed as a simple report. The estimator can create a public shared link to allow anonymous view access to the report. The report can also be downloaded as a PDF.

### Contingency

Contingency percentage rate is the confidence level in the information used to create the estimate. The contingency percentage rate is used to padded the estimate with a percentage rate.

### Complexity

Tasks and Factors use following complexity scale.

* **Very Simple** - Task is trivial, owner knows how to solve the problem and can be done quickly.
* **Simple** - Owner knows how to solve the problem.
* **Medium** - Owner needs to do a little bit of research to solve the problem, but the resulting solution is not complex.
* **Complex** - Task needs research and some clarification on details. Resulting solution is not trivial.
* **Very Complex** - Task needs research and clarification. Resulting solution requires significant new work or change.

### Organizations

Projects and Templates are placed in an organization. All members of the organization can edit the Project or Template.

Select 'Private' to make the Project or Template accessible by only you.

### Templates

A template is a group of factors you can quickly add to a project. Templates allow reuse of common factors across projects.