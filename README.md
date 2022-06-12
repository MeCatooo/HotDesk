# HotDesk app for SoftwareMind-Intern-Challenge

Application is an API for management Hot desk system which allows to:

Administration:
- Manage locations (add/remove, can't remove if desk exists in location) - (GET,POST, DELETE) api/Locations
- Manage desk in locations (add/remove if no reservation/make unavailable) - (GET,POST, DELETE, PATCH) api/Locations/{id}/desk

Employees
- Determine which desks are available to book or unavailable. - api/Locations/{id}/(occupied, free)
- Filter desks based on location - GET api/Locations/{id}/desks
- Book a desk for the day. - POST api/Reservations/location/{id}/desk/{deskId}
- Allow reserving a desk for multiple days but now more than a week. - POST api/Reservations/location/{id}/desk/{deskId}
- Allow to change desk, but not later than the 24h before reservation. - PATCH api/Reservations/
- Administrators can see who reserves a desk in location, where Employees can see only that specific
desk is unavailable. - GET api/Reservations - This method works diffrently depending on the logged in user role.

Additional functionality:

Login:

- Register - POST api/Login/Register
- Login, generates Bearer token - POST api/Login

User:

- Set user role (Administrator or User) - PATCH api/User/set/role

Technologies used in project:

- EF core and SQlite for storage and easy setup.
- JWToken for authorization.
- Swagger for easy api testing.
- EF InMemory for unit tests.

Setup:

- Launch clonned project.

My other similar project:

- https://github.com/MeCatooo/Projekt-ASP - WebApp created in ASP.NET MVC
- https://github.com/MeCatooo/Adam-Turek-pab/tree/main/Projekt_zaliczeniowyV2 - Restaurant API in Noje.js (type-script)
