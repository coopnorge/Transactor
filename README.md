# Transactor
> Workshop designed to educate about refactoring and code design.

### 📃 Instructions 

1. Add missing unit tests for `TransactionManager` and `ProcessorManager`.
2. Define common data models and integrate it to current solution.
3. Refactor code base to be more structured and polymorphic.
4. Develop chained bank account usage for payments.

#### Structure of project
```text
Transactor
└───src
    └───Terminal            // Application C# project
        ├───Core
        │   ├───Clients     // Third party client's (API's)
        │   ├───Exceptions  // Application exception logic
        │   ├───Extensions  // Extensions for application methods, C# types
        │   ├───Generators  // Any kind of generators
        │   └───Managers    // Managers to handle specific tasks (transactions, accounts)
        └───Infrastructure
            └───Storage     // Contains code where data stored
```

### ✔️Acceptance Criteria
Transactions must use bank accounts for payments.
So if we cannot pay from one account then we must use another account balance to pay.

As a result that will be visible in logs:
```text
| Bank account | Balance |
| CapsBank | 100.00 |
| BitBank | 50.00 |

info: Terminal.Core.Managers.TransactionManager[0]
      New transaction: 5/4/2021 12:45:36 AM = 50
info: Terminal.Core.Managers.TransactionManager[0]
      New transaction: 5/4/2021 12:45:35 AM = 50
info: Terminal.Core.Managers.ProcessorManager[0]
      [SUCCESS] Processing transaction via BitBank: -50
fail: Terminal.Core.Managers.ProcessorManager[0]
      [FAILED] Processing transaction via BitBank: -50
fail: Terminal.Core.Managers.ProcessorManager[0]
      [SUCCESS] Processing transaction via CapsBank: -50
info: Terminal.Core.Application[0]

| Bank account | Balance |
| CapsBank | 50.00 |
| BitBank | 0.00 |
```

1. Must compile and all tests must pass.
2. Well-structured code base.
3. Bank accounts must be used in the chain.

### Quick start

Use the .NET Core CLI to run the application and tests.

**Run Program**
```shell
dotnet run --project "./src/Terminal/Terminal.csproj"
```
**Run Tests**
```shell
dotnet test test/Terminal.Test.Unit/Terminal.Test.Unit.csproj
```
