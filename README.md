# EstimatorX

![EstimatorX](https://raw.githubusercontent.com/loresoft/Estimatorx/develop/design/full-logo.svg)

Fast, consistent project estimates that account for uncertainty.

[http://estimatorx.com](http://estimatorx.com "EstimatorX")

## EstimatorX Features

### Projects

A project holds everything that makes up an estimate. Work is organized into Epics, which are broken down into Features.

### Organizations

Projects and Templates belong to an organization, and every member of that organization can edit them.

Select **Private** if only you should have access to a Project or Template.

### Epics

An Epic is a large deliverable that provides business value. It's usually the largest unit of work in a project and groups one or more related Features.

### Features

A Feature is a piece of an Epic that can be delivered on its own. Together, the Features make up the Epic, and each should provide business value that supports it.

### Templates

A template is a reusable set of Epics, Features, and project Settings. Use templates to share common work and settings, such as estimate values, risk and effort scales, and the multiplier matrix, across projects.

### Settings

Each estimate has its own Settings. Customize the estimate values, risk and effort scales, and multiplier matrix to fit the project without affecting other estimates.

### Assumptions

List the assumptions behind each estimate. If an assumption later turns out to be wrong, the record makes it easy to see which estimates need another look.

### Estimate

The base estimate for a Feature, before any adjustment. You can customize the available values in the project settings.

### Weighted Estimate

The Estimate multiplied by the Multiplier. This adjusted value accounts for uncertainty and is used in project totals.

### Clarity

How well is the Feature defined, and how well do you understand it? This measures understanding, not how simple the work is.

- **Low** No documentation, and the Feature hasn't been discussed with the customer.
- **Medium Low** Little documentation, and the customer has only briefly mentioned the Feature.
- **Medium** Little documentation, but the customer has walked through the desired functionality.
- **Medium High** Some documentation; the functionality is understood but not fully written down.
- **High** Detailed documentation, including technical requirements and examples. The desired outcome and acceptance criteria are well understood.

### Confidence

How confident are you in the estimate, given what you know? Have you done something like this before? Do you know all the technologies involved, and have they all been identified?

- **Low** The solution isn't known yet, and more discovery is required.
- **Medium Low** The approach isn't clear, and research or a proof of concept will be required. The requirements suggest the work is feasible.
- **Medium** You've done related but not very similar work. There are possible approaches but no clear direction, so some research and discussion are needed.
- **Medium High** You've done similar work and have a good idea of the technology and approach.
- **High** You've done this before and are confident in the numbers.

### Multiplier

A computed factor that pads the Estimate based on the Feature's Clarity and Confidence. You can customize the multiplier matrix in the project settings.

### Risk

Shows how risky a Feature is, based on its computed Multiplier. You can customize the Risk Scale in the project settings.

### Effort

Shows a Feature's overall size, based on its Estimate. You can customize the Effort Scale in the project settings.

## Docker

EstimatorX is published as a container image to the GitHub Container Registry for `linux/amd64` and `linux/arm64`.

```shell
docker run -d \
  --name estimatorx \
  -p 8080:8080 \
  -v estimatorx-data:/home/app \
  ghcr.io/loresoft/estimatorx:latest
```

Then browse to `http://localhost:8080`.

### Image Tags

| Tag            | Description                             |
| -------------- | --------------------------------------- |
| `latest`       | Latest release                          |
| `X.Y.Z`, `X.Y` | Release versions from `vX.Y.Z` git tags |
| `master`       | Latest build of the `master` branch     |
| `develop`      | Latest build of the `develop` branch    |
| `sha-<commit>` | Build of a specific commit              |

### Persisting Data

By default the container uses SQLite at `/home/app/data/EstimatorX.db`. Data protection keys are stored in the database, so users stay signed in across restarts. The container runs as the non-root `app` user (UID `1654`).

Only the SQLite provider needs a volume to persist data. PostgreSQL and SQL Server store everything, including data protection keys, in the external database.

- **Named volume** - mount the volume at `/home/app` so it inherits the `app` user ownership from the image.

  ```shell
  docker run -d \
    -p 8080:8080 \
    -v estimatorx-data:/home/app \
    ghcr.io/loresoft/estimatorx:latest
  ```

- **Bind mount** - the host directory must be writable by UID `1654`.

  ```shell
  mkdir -p ./data && sudo chown -R 1654:1654 ./data
  docker run -d \
    -p 8080:8080 \
    -v ./data:/home/app/data \
    ghcr.io/loresoft/estimatorx:latest
  ```

### Configuration

Configure the container with environment variables.

#### General

| Variable                | Description                          | Default |
| ----------------------- | ------------------------------------ | ------- |
| `ASPNETCORE_HTTP_PORTS` | HTTP port the application listens on | `8080`  |

#### Database

| Variable                     | Description                                              | Default                                    |
| ---------------------------- | -------------------------------------------------------- | ------------------------------------------ |
| `Database__Provider`         | Database provider: `sqlite`, `sqlserver` or `postgresql` | `sqlite`                                   |
| `Database__ConnectionString` | Database connection string                               | `Data Source=/home/app/data/EstimatorX.db` |
| `Database__MigrateOnStartup` | Apply database migrations on startup                     | `true`                                     |

Set both `Database__Provider` and `Database__ConnectionString` when using a provider other than SQLite.

##### SQLite

Mount a volume so the database, including data protection keys, survives container restarts, see [Persisting Data](#persisting-data).

```shell
docker run -d \
  -p 8080:8080 \
  -v estimatorx-data:/home/app \
  ghcr.io/loresoft/estimatorx:latest
```

To use a different location, set `Database__ConnectionString` to a path writable by the `app` user (UID `1654`).

```shell
docker run -d -p 8080:8080 \
  -v estimatorx-data:/home/app \
  -e Database__ConnectionString="Data Source=/home/app/db/EstimatorX.db" \
  ghcr.io/loresoft/estimatorx:latest
```

##### PostgreSQL

No volume is needed; all data, including data protection keys, is stored in PostgreSQL.

```shell
docker run -d -p 8080:8080 \
  -e Database__Provider=postgresql \
  -e Database__ConnectionString="Host=postgres;Database=EstimatorX;...>" \
  ghcr.io/loresoft/estimatorx:latest
```

##### SQL Server

No volume is needed; all data, including data protection keys, is stored in SQL Server.

```shell
docker run -d -p 8080:8080 \
  -e Database__Provider=sqlserver \
  -e Database__ConnectionString="Server=sqlserver;Database=EstimatorX;..." \
  ghcr.io/loresoft/estimatorx:latest
```

#### Authentication

EstimatorX uses external login providers for sign in. At least one provider must be configured, otherwise users will not be able to sign in.

| Variable                  | Description                           |
| ------------------------- | ------------------------------------- |
| `Microsoft__ClientId`     | Microsoft account login client id     |
| `Microsoft__ClientSecret` | Microsoft account login client secret |
| `Google__ClientId`        | Google login client id                |
| `Google__ClientSecret`    | Google login client secret            |
| `GitHub__ClientId`        | GitHub login client id                |
| `GitHub__ClientSecret`    | GitHub login client secret            |
| `Facebook__AppId`         | Facebook login app id                 |
| `Facebook__AppSecret`     | Facebook login app secret             |

A login provider is enabled only when both of its values are set.

```shell
docker run -d -p 8080:8080 \
  -v estimatorx-data:/home/app \
  -e GitHub__ClientId=<client-id> \
  -e GitHub__ClientSecret=<client-secret> \
  ghcr.io/loresoft/estimatorx:latest
```

#### Email

Email is sent over SMTP and is required for sending organization invites.

| Variable             | Description                                  | Default      |
| -------------------- | -------------------------------------------- | ------------ |
| `Email__Server`      | SMTP server host name                        |              |
| `Email__Port`        | SMTP server port                             | `465`        |
| `Email__UseSSL`      | Use SSL when connecting to the SMTP server   | `true`       |
| `Email__UserName`    | SMTP authentication user name                |              |
| `Email__Password`    | SMTP authentication password                 |              |
| `Email__FromAddress` | Sender email address, required to send email |              |
| `Email__FromName`    | Sender display name                          | `EstimatorX` |

```shell
docker run -d -p 8080:8080 \
  -v estimatorx-data:/home/app \
  -e Email__Server=smtp.example.com \
  -e Email__Port=465 \
  -e Email__UserName=estimatorx@example.com \
  -e Email__Password=<password> \
  -e Email__FromAddress=estimatorx@example.com \
  ghcr.io/loresoft/estimatorx:latest
```

#### OpenTelemetry

Logs, traces and metrics are exported with OpenTelemetry using the OTLP exporter. Configure the exporter with the standard OpenTelemetry environment variables.

| Variable                      | Description                                                       | Default |
| ----------------------------- | ----------------------------------------------------------------- | ------- |
| `OTEL_SERVICE_NAME`           | Service name reported with telemetry                              |         |
| `OTEL_EXPORTER_OTLP_ENDPOINT` | OTLP collector endpoint, for example `http://otel-collector:4317` |         |
| `OTEL_EXPORTER_OTLP_PROTOCOL` | OTLP protocol: `grpc` or `http/protobuf`                          | `grpc`  |
| `OTEL_EXPORTER_OTLP_HEADERS`  | Additional headers sent to the collector, such as an API key      |         |
| `OTEL_RESOURCE_ATTRIBUTES`    | Additional resource attributes, `key1=value1,key2=value2`         |         |

```shell
docker run -d -p 8080:8080 \
  -v estimatorx-data:/home/app \
  -e OTEL_SERVICE_NAME=EstimatorX \
  -e OTEL_EXPORTER_OTLP_ENDPOINT=http://otel-collector:4317 \
  -e OTEL_EXPORTER_OTLP_PROTOCOL=grpc \
  ghcr.io/loresoft/estimatorx:latest
```

### HTTPS

The container listens on HTTP only. Authentication cookies require a secure connection, so run the container behind a TLS terminating reverse proxy (nginx, Traefik, Caddy, Azure Container Apps, etc.). Forwarded headers from the proxy are honored. Plain HTTP works only when browsing to `localhost`.

### Building the Image

The image is built with the .NET SDK container support, no Dockerfile is required. Requires the .NET 10 SDK, Docker and [dart-sass](https://sass-lang.com/dart-sass/) (`npm i -g sass`).

```shell
dotnet publish src/EstimatorX.Web/EstimatorX.Web.csproj -c Release -t:PublishContainer
```

The image is created in the local Docker daemon as `loresoft/estimatorx`.

## Running from the Zip

Each build produces `EstimatorX.Web.zip`, a framework-dependent build of the application, available as the `Application` artifact on the GitHub Actions run. It runs on Windows, Linux and macOS.

The zip contains compiled output, not source, so start it with `dotnet EstimatorX.Web.dll` rather than `dotnet run`. Requires the [ASP.NET Core 10 Runtime](https://dotnet.microsoft.com/download/dotnet/10.0).

Extract the zip and run from the extracted folder.

```shell
unzip EstimatorX.Web.zip -d estimatorx
cd estimatorx

export Database__ConnectionString="Data Source=./data/EstimatorX.db"
export GitHub__ClientId=<client-id>
export GitHub__ClientSecret=<client-secret>

dotnet EstimatorX.Web.dll --urls "http://localhost:8080"
```

PowerShell

```powershell
Expand-Archive EstimatorX.Web.zip -DestinationPath estimatorx
Set-Location estimatorx

$env:Database__ConnectionString = "Data Source=./data/EstimatorX.db"
$env:GitHub__ClientId = "<client-id>"
$env:GitHub__ClientSecret = "<client-secret>"

dotnet EstimatorX.Web.dll --urls "http://localhost:8080"
```

Then browse to `http://localhost:8080`.

- All settings in the [Configuration](#configuration) section apply. Set them as environment variables, command line arguments (`--Database:Provider=postgresql`) or in an `appsettings.Production.json` file next to `EstimatorX.Web.dll`.
- Set `Database__ConnectionString` when using SQLite; the built-in default `/home/app/data/EstimatorX.db` targets the container image layout. The database directory is created automatically if it does not exist.
- Data protection keys are stored in the configured database.
- Use `--urls` or `ASPNETCORE_URLS` to change the listening address. The same [HTTPS](#https) guidance applies when hosting outside of `localhost`.

To run from source instead, use `dotnet run --project src/EstimatorX.Web/EstimatorX.Web.csproj`. Requires the .NET 10 SDK and dart-sass.
