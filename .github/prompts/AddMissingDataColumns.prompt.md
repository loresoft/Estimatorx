---
description: Add hidden DataColumns for TItem properties missing from a LoreSoft DataGrid
agent: agent
argument-hint: Open the .razor file that contains the DataGrid
---

## Task

Update each `<DataGrid TItem="...">` in `${file}` by adding a hidden `<DataColumn>` for every eligible `TItem` property that does not already have a column.

1. Find the class named by `TItem` in the workspace and read its properties.
2. If the file has multiple `DataGrid`s, process each one independently against its own `TItem`.
3. Do not modify, reorder, or remove existing columns. Only insert new ones.

## 1. Eligible Properties

Include only public instance properties declared in the `TItem` class body. Skip properties inherited from base classes (for example, `EntityReadModel`).

Skip a property if any of these are true:

- Name is `PeriodStart`, `PeriodEnd`, or `RowVersion`.
- Name contains `Secret`, `Token`, `Password`, or `Hash`, or ends with `Key` (case-insensitive). Examples: `ApiToken`, `ClientSecret`, `SecurityKey`.
- Type is complex: collections (`List<T>`, arrays, `IEnumerable<T>`, etc.) or nested classes (for example, `ProjectSettings`). Only simple types from the table in section 3 get columns.
- A `<DataColumn>` in the grid already has `Property="x => x.PropName"`, including columns with a `<Template>`. For example, an Action column bound to `x => x.Id` means `Id` is already present.

## 2. Insertion Location

Insert all new columns immediately before the first `<DataColumn Property="x => x.CreatedBy" ...>`. If there is no `CreatedBy` column, append them at the end of `<DataColumns>`.

Order within the inserted block:

- Follow the property declaration order in `TItem`.
- Keep related properties adjacent, with the identifier first. For example: `OrganizationId`, `OrganizationName`; `UserId`, `UserName`, `UserEmail`.

## 3. Column Format

Each new column has:

- `Property="x => x.PropName"`
- `Title` with PascalCase split into words (`EstimatedCost` → `"Estimated Cost"`)
- Type-specific attributes from the table below
- `Visible="false"`
- No `<Template>`

Attribute order: `Property`, `Title`, `Width`, `Align`, `Format`, `Visible`.

Match the file's existing layout: same indentation, one attribute per line aligned under the first, and a blank line between columns.

### Type-specific attributes

Unwrap `Nullable<T>` and match on the underlying type.

| Type                                                  | Width   | Align             | Format |
| ----------------------------------------------------- | ------- | ----------------- | ------ |
| `int`, `long`, `short`, `byte`                        | `100px` | `TextAlign.Right` |        |
| `double`, `float`                                     | `100px` | `TextAlign.Right` | `N2`   |
| `decimal`, name contains `Cost`, `Price`, or `Amount` | `150px` | `TextAlign.Right` | `C`    |
| `decimal`, other names                                | `150px` | `TextAlign.Right` | `N2`   |
| `DateTime`, `DateOnly`                                | `110px` |                   | `d`    |
| `DateTimeOffset`                                      | `180px` |                   | `g`    |
| `TimeOnly`                                            | `100px` |                   | `t`    |
| `TimeSpan`                                            | `100px` |                   | `c`    |
| `bool`                                                | `110px` |                   |        |
| `Guid`                                                | `300px` |                   |        |
| enum                                                  | `150px` |                   |        |
| `string`                                              |         |                   |        |

Leave out any attribute whose cell is empty.

### Example

```razor
<DataColumn Property="x => x.OrganizationId"
            Title="Organization Id"
            Width="100px"
            Align="TextAlign.Right"
            Visible="false" />

<DataColumn Property="x => x.OrganizationName"
            Title="Organization Name"
            Visible="false" />

<DataColumn Property="x => x.EstimatedCost"
            Title="Estimated Cost"
            Width="150px"
            Align="TextAlign.Right"
            Format="C"
            Visible="false" />
```

## 4. Checklist

- Every eligible property has exactly one column.
- Every new column is `Visible="false"` and has no template.
- Existing columns are unchanged.
