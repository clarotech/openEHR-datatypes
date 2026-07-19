# Clarotech.OpenEHR.RM.Datatypes

A C# implementation of the [openEHR Reference Model Release 1.1.0 Data Types](https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html) specification.

## Packages

| Package | Namespace | Classes |
|---|---|---|
| `data_types.basic` | `OpenEHR.RM.DataTypes.Basic` | `DataValue`, `DvBoolean`, `DvIdentifier`, `DvState` |
| `data_types.text` | `OpenEHR.RM.DataTypes.Text` | `DvText`, `DvCodedText`, `CodePhrase`, `TermMapping`, `TerminologyId`, `DvParagraph` (deprecated) |
| `data_types.quantity` | `OpenEHR.RM.DataTypes.Quantity` | `DvOrdered`, `DvOrdinal`, `DvScale`, `DvQuantified`, `DvAmount`, `DvAbsoluteQuantity`, `DvQuantity`, `DvCount`, `DvProportion`, `DvInterval<T>`, `ReferenceRange<T>` |
| `data_types.date_time` | `OpenEHR.RM.DataTypes.DateTime` | `DvTemporal`, `DvDate`, `DvTime`, `DvDateTime`, `DvDuration` |
| `data_types.time_specification` | `OpenEHR.RM.DataTypes.TimeSpecification` | `DvTimeSpecification`, `DvPeriodicTimeSpecification`, `DvGeneralTimeSpecification` |
| `data_types.uri` | `OpenEHR.RM.DataTypes.Uri` | `DvUri`, `DvEhrUri` |
| `data_types.encapsulated` | `OpenEHR.RM.DataTypes.Encapsulated` | `DvEncapsulated`, `DvMultimedia`, `DvParsable` |

## Quick start

```csharp
using OpenEHR.RM.DataTypes.Basic;
using OpenEHR.RM.DataTypes.Text;
using OpenEHR.RM.DataTypes.Quantity;
using OpenEHR.RM.DataTypes.DateTime;

// Boolean
var flag = new DvBoolean(true);

// Coded text
var terminology = new TerminologyId("SNOMED-CT");
var code = new CodePhrase(terminology, "38341003", "Hypertension");
var diagnosis = new DvCodedText("Hypertension", code);

// Quantity with units
var weight = new DvQuantity(72.5, "kg", precision: 1);

// ISO 8601 date (supports partial dates)
var dob = new DvDate("1985-03-15");
var yearOnly = new DvDate("1985");

// Duration arithmetic
var duration = new DvDuration("P1Y6M");
```

## Versioning

The library targets openEHR RM Release-1.1.0. The major.minor version (`1.1.x`) matches that RM release; the patch number increments independently for library fixes and additions that don't change the targeted spec release.

## License

Apache-2.0
