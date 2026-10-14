---
description: Add missing EditForm inputs for the update model of an EditPageBase page
agent: agent
argument-hint: Open the .razor page containing the EditForm to update
---

# Task: Add Missing `EditForm` Inputs for Update Model Properties

Update the `EditForm` in `${file}` to include an input for every eligible property of the page's update model. The page can have any name (for example, `Edit.razor` or `Profile.razor`).

## Locating the Correct Model

1. Find the `@inherits` directive at the top of `${file}`. It will follow this pattern:
    ```razor
    @inherits EditPageBase<{TKey}, {Prefix}ReadModel, {Prefix}UpdateModel>
    ```
    For example:
    ```razor
    @inherits EditPageBase<long, PartnerReadModel, PartnerUpdateModel>
    ```
2. The **third** type argument (e.g., `PartnerUpdateModel`) is the **update model** — this is the source of truth for which properties to include in the form.
3. Locate the C# class file for that update model (e.g., `PartnerUpdateModel.cs`) by searching the solution for `{Prefix}UpdateModel.cs`. It is typically in the shared project under a `Domain/{Prefix}/Models/` folder.
4. Enumerate its **public instance properties**, including properties inherited from base classes. Inherited audit and concurrency properties are removed by the exclusions below.

> The `{Prefix}` (e.g., `Partner`) will differ per usage. Always derive it from the `@inherits` directive of the calling page.

## Exclusions

Exclude the following properties:

- `Id`
- `Created`
- `CreatedBy`
- `Updated`
- `UpdatedBy`
- `PeriodStart`
- `PeriodEnd`
- `RowVersion`

Also exclude:

- Any property that already has an input bound to it (`@bind-Value="Store.Model!.{PropertyName}"`).
- Complex types: collections (`List<T>`, arrays, `IEnumerable<T>`, etc.) and nested classes (for example, `ProjectSettings`). Only generate inputs for simple types: strings, numbers, booleans, dates, and their nullable forms.

## Rules

1. **Do not modify or move existing input components already in the file.**
2. Use the template below when generating new inputs. Replace `{PropertyName}` with the real property name.
3. For `{PropertyName|Title}`, split the PascalCase property name into **Title Case** words (e.g., `HostMatches` → `Host Matches`, `EmailAddress` → `Email Address`).
4. Replace `InputText` in the template with the correct component based on property type, and adjust attributes as listed:

    | Property                                                       | Component                                         | Attribute changes                           |
    | -------------------------------------------------------------- | ------------------------------------------------- | ------------------------------------------- |
    | Text                                                           | `InputText`                                       | none                                        |
    | Long text (names containing `Description`, `Notes`, `Comment`) | `InputTextArea`                                   | add `rows="5"` after `class`                |
    | Email (names containing `Email`)                               | `InputText`                                       | add `type="email"` before `class`           |
    | URL (names containing `Url`, `Uri`, `Website`)                 | `InputText`                                       | add `type="url"` before `class`             |
    | Number types                                                   | `InputNumber`                                     | none                                        |
    | Boolean types                                                  | `BooleanSelect` (from `LoreSoft.Blazor.Controls`) | `class="form-select"`; remove `placeholder` |
    | Date types                                                     | `InputDate`                                       | remove `placeholder`                        |

5. If the property is **nullable**, omit the `<span class="text-danger" title="Required Field">*</span>` element from the label.
6. Replace `{HelpText}` with one short sentence describing what the field is for. If nothing useful can be said beyond the label, omit the entire `<div class="form-text">` element.

## Layout

1. Place new inputs inside the existing columns of the `<!-- Edit Form -->` row (for example, two `col-md-6` columns, or `col` columns). Keep any existing inputs where they are and add new inputs after them.
2. Group related inputs together in the same column (for example, `Name` and `Description`; `StartDate` and `EndDate`). Balance the number of inputs between columns.
3. If the existing columns become too long (more than about 6 inputs each), put the remaining groups in a new row directly after the `<!-- Edit Form -->` row. Use the same column structure, and add a `<!-- {Group Name} -->` comment above the row.
4. Match the file's existing indentation.

## Template

```razor
<div class="mb-3">
    <label for="{PropertyName}" class="form-label">
        {PropertyName|Title}: <span class="text-danger" title="Required Field">*</span>
    </label>
    <InputText DisplayName="{PropertyName|Title}"
               @bind-Value="Store.Model!.{PropertyName}"
               id="{PropertyName}"
               name="{PropertyName}"
               class="form-control"
               placeholder="{PropertyName|Title}" />
    <ValidationMessage For="@(() => Store.Model.{PropertyName})" />
    <div class="form-text">
        {HelpText}
    </div>
</div>
```
