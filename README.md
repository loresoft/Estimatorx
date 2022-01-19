# EstimatorX

![EstimatorX](https://raw.githubusercontent.com/loresoft/Estimatorx/develop/design/full-logo.svg)

A simple project estimation application.

[http://estimatorx.com](http://estimatorx.com "EstimatorX")

## EstimatorX Features

### Projects

A project contains all the details that make up an estimate. An estimate is broken down into Epics and Features.

### Epics

A high level deliverable that provides business value, this is the largest units of work in most projects and groups together one or more features.

### Features

Features are broken down components that are individually deliverable, and when combined comprise the epic. A feature should provide business value in support of an Epic.

### Assumptions

When making an estimate, there are assumptions the estimator makes to come up with the estimate. Document those assumptions to help raise the red flag in the future when an assumptions proves not to be true.

### Estimate

The estimated value for the feature. The available values are customizable via the project settings.

### Clarity

How well has the feature been outlined and defined? How well do you understand it? This is not a "simplicity" call - this is a measure for understanding.

- **Low** There is no documentation, the item has not have even been covered with the customer
- **Medium Low** There is not much documentation, the item may not have even been covered with the customer
- **Medium** There is not much documentation, but the customer was able to walk through the desired functionality
- **Medium High** There is some documentation, the desired functionality is understood but may not be well documented
- **High** There is extensive documentation, up to and including technical requirements, examples, the desired outcome is well understood and acceptance criteria is understood

### Confidence

How confident/comfortable are you in the estimate based on everything you know? Have you done something like this before? Are you familiar with all technologies involved? Have they all been identified?

- **Low** Solution is not know and more discovery is required
- **Medium Low** Approach is not clear and research / proof of concept WILL be required. Current understanding of the requirements support reasonable feasibility for this work being achievable.
- **Medium** Haven’t done something super similar, but have done stuff related. Have ideas of approaches but no clear direction, some research and discussion will be needed to pick an approach
- **Medium High** Have done similar but not this, have an idea of the technology and approach we would use
- **High** Have done it before, pretty sure on the numbers

### Multiplier

Multiplier is a computed value used to pad the estimate based on the Clarity and Confidence of the feature. The multiplier matrix is customizable via the project settings

### Weighted Estimate

The estimated value times the multiplier is the weighted estimate.

### Risk

How risky is the feature, based on the computed multiplier. The Risk Scale can be customized via the project settings.

### Effort

What is the high level effort of the feature, based on the estimate. The Effort Scale can be customized via the project settings.

### Organizations

Projects and Templates are placed in an organization. All members of the organization can edit the Project or Template.

Select 'Private' to make the Project or Template accessible by only you.

### Templates

A template is a project pre-configured with settings and optionally epics and features. Use templates as a quick way to get started with an estimate based on standard settings.
