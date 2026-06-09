# Large Fantasy Model (VTT & RPG Platform)

Projekt to interaktywna aplikacja webowa do przeprowadzania sesji RPG online (Virtual Tabletop). Umożliwia graczom rejestrację, dołączanie do pokoi (lobby), komunikację w czasie rzeczywistym oraz interakcję na planszy gry.

## Wykorzystane Technologie i Biblioteki

Aplikacja została zbudowana w oparciu o architekturę **ASP.NET Core MVC** (wersja .NET 8).

### Backend (NuGet)
* **Microsoft.EntityFrameworkCore (v8.0.0)** - Główny system ORM (Object-Relational Mapping) do zarządzania bazą danych.
* **Microsoft.EntityFrameworkCore.SqlServer (v8.0.0)** - Provider bazy danych dla Microsoft SQL Server.
* **Microsoft.EntityFrameworkCore.Tools (v8.0.0)** - Narzędzia wspierające m.in. tworzenie i aplikowanie migracji z poziomu konsoli.
* **Microsoft.EntityFrameworkCore.Design (v8.0.0)** - Komponent używany podczas projektowania bazy (Command Line Tools).
* **BCrypt.Net-Next (v4.2.0)** - Zaawansowana biblioteka kryptograficzna używana do bezpiecznego hashowania oraz weryfikacji haseł użytkowników.
* **Swashbuckle.AspNetCore (v10.2.1)** - Biblioteka do automatycznego generowania dokumentacji API i interfejsu Swagger UI.
* **SignalR** (wbudowane w ASP.NET Core) - Framework wykorzystywany do komunikacji w czasie rzeczywistym (Czat, ruchy po planszy, statusy lobby).

### Frontend (wwwroot/lib)
* **Bootstrap** - Framework CSS do tworzenia responsywnych interfejsów (UI).
* **jQuery** - Biblioteka JavaScript usprawniająca manipulację modelem DOM (DOM manipulation) i zapytania AJAX.
* **jQuery Validation** (oraz `jquery-validation-unobtrusive`) - Wtyczki do weryfikacji poprawności danych w formularzach bezpośrednio po stronie klienta (Client-side validation).

---

## Wymagania Wstępne
Aby uruchomić projekt na swoim środowisku lokalnym, musisz posiadać:
1. **.NET 8.0 SDK**
2. **SQL Server Express LocalDB** (instalowany domyślnie np. z Visual Studio) lub inny serwer SQL Server.
3. Edytor kodu (zalecane: Visual Studio 2022 lub JetBrains Rider).

---

## Instrukcja Instalacji i Konfiguracji

### 1. Klonowanie repozytorium
Pobierz projekt na swój dysk:
```bash
git clone <https://github.com/piotr-kluczynski/large-fantasy-model>
cd large-fantasy-model/large-fantasy-model
```

### 2. Konfiguracja bazy danych
Domyślnie aplikacja jest skonfigurowana pod LocalDB z bazą o nazwie `LargeFantasyModelDB`.
Jeżeli używasz standardowej instalacji Visual Studio, nie musisz zmieniać `ConnectionStrings` w pliku `appsettings.json`.

Jeżeli posiadasz własny serwer SQL, zaktualizuj wpis w pliku `appsettings.json`:
```json
"ConnectionStrings": {
	"LargeFantasyModelDB": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LargeFantasyModelDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
}
```

### 3. Konfiguracja zewnętrznego API (Gemini)
Aplikacja wykorzystuje model AI (Gemini). W pliku `appsettings.json` lub `secrets.json` musisz uzupełnić klucz API w odpowiedniej sekcji, w przeciwnym razie funkcje oparte na AI mogą rzucać błędy:
```json
"Gemini": {
  "ApiKey": "Klucz_Api"
}
```

### 4. Wykonanie Migracji i Utworzenie Bazy Danych
Otwórz terminal (lub konsolę Package Manager Console w Visual Studio) w katalogu zawierającym plik `large-fantasy-model.csproj` i wykonaj polecenia:

**(Dla .NET CLI / Terminal):**
```bash
dotnet ef database update
```
**(Dla Package Manager Console w Visual Studio):**
```powershell
Update-Database
```
Polecenie to stworzy nową bazę danych oraz odpowiednie tabele potrzebne m.in. dla systemu logowania (rejestracji).

### 5. Uruchomienie Aplikacji
Wystarczy uruchomić projekt z poziomu środowiska graficznego (F5 w Visual Studio) lub w terminalu, używając polecenia:
```bash
dotnet run
```
Aplikacja po zbudowaniu wskaże adres (najczęściej `http://localhost:5218` lub `https://localhost:7248`), pod którym można uzyskać do niej dostęp z poziomu przeglądarki.
