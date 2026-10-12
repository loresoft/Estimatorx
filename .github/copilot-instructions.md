# Copilot Instructions

## Project Guidelines
- Avoid multiple complex statements on one line. Extract complex arguments (e.g., object initializers, collection literals) into named local variables before passing them to a method call. Example: instead of `var service = CreateService(toaster, new NotificationServiceOptions { ErrorTimeout = 42 });`, write `var options = new NotificationServiceOptions { ErrorTimeout = 42 };` then `var service = CreateService(toaster, options);`
