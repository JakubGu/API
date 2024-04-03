## Endpointy API

### POST /api/auth/login

- **Parametry**: Obiekt `LoginDto` zawierający `Email` i `Password`. [FromBody]
- **Opis**: Autentykuje użytkownika. ustawia token autoryzacji w nagłówkach odpowiedzi.

### POST /api/auth/register

- **Parametry**: Obiekt `RegisterDto` zawierający `Email`, `UserName` i `Password`. [FromBody]
- **Opis**: Rejestruje nowego użytkownika. ustawia token autoryzacji w nagłówkach odpowiedzi.

### GET /api/account/currentuser

- **Parametry**: Brak.
- **Opis**: Zwraca nazwę dla aktualnie zalogowanego użytkownika.

### GET /api/tags

- **Parametry**: Obiekt `TagParamsDto` zawierający `PageNumber`, `PageSize`, `SortBy` i `OrderBy`. [FromQuery]
- **Opis**: Zwraca listę tagów. Metoda najpierw tworzy nowy obiekt `TagsList.Command` z parametrami przekazanymi w `TagParamsDto`. Następnie wysyła ten obiekt do mediatora, który zwraca listę tagów.

### POST /api/tags/refresh

- **Parametry**: Brak.
- **Opis**: Odświeża listę tagów. Metoda najpierw wysyła do mediatora `TagsDelete.Command`, który usuwa wszystkie tagi, a następnie wysyła `TagsAdd.Command`, który dodaje tagi.

### GET /swagger/v1/swagger.json

- **Parametry**: Brak.
- **Opis**: Zwraca dokumentację API w formacie JSON.

## Tokeny

### Użytkowanie Tokenu

- **Opis**: Po pomyślnym zalogowaniu lub rejestracji, użytkownik otrzymuje token w nagłówku `Authorization` jako `Bearer token`, który musi być przesyłany w nagłówku `Authorization` jako `Bearer token` przy każdym zapytaniu do API.
- **Parametry**: Tokeny są generowane przez serwer i zawierają informacje o użytkowniku, takie jak jego nazwa użytkownika, e-mail i ID. Tokeny mają określony czas ważności 24 godziny, po którym muszą być odnowione.

## Punkty z zadania

### Pobrać min. 1000 tagów..... / Pobrane może nastąpić na starcie.....

- **Opis**: Po uruchomieniu aplikacji, tworzona jest baza danych z ostatniej migracji i jest zasiewana danymi z klasy `Seed`.

### Obliczyć procentowy udział tagów.....

- **Opis**: W klasie Seed oraz w mediatorze TagAdd, dodana jest metoda `Percentage = (tagsCountSum != 0 && item.count != 0) ? Math.Round((decimal)item.count / tagsCountSum * 100, 2) : 0`, służąca do obliczania procentowego udziału tagów.

### Udostępnić tagi poprzez stronicowane API.....

- **Opis**: Endpoint `/api/tags` oraz mediator `TagsList` obsługują te wymagania.

### Udostępnić metodę API do wymuszenia ponownego.....

- **Opis**: Endpoint `/api/tags/refresh` oraz mediatory `TagsAdd`, `TagsDelete` obsługują te wymagania.

### Udostępnić definicję OpenAPI.....

- **Opis**: Endpoint `/swagger/v1/swagger.json` obsługuje te wymagania.

### Uwzględnić logowanie oraz obsługę błędów i konfigurację.....

- **Opis**: Kontroler `Account` który używa `Identity` oraz tokeny obsługuje logowanie. Obsługa błędów jest dodana do każdego endpointa. Konfiguracja uruchomieniowa znajduje się w plikach `appsettings.Development.json` oraz `appsettings.json`.

### Przygotować kilka wybranych testów jednostkowych.....

- **Opis**: Klasa `PasswordComplexityAttributeTests` spełnia te wymagania.

### Przygotować kilka wybranych testów integracyjnych.....

- **Opis**: Klasa `TagControllerTests` spełnia te wymagania.

### Wykorzystać konteneryzację do zapewnienia.....

- **Opis**: Plik `Dockerfile` obsługuje te wymagania.

### Całość powinna się uruchamiać po wykonaniu.....

- **Opis**: Plik `docker-compose` obsługuje te wymagania.
