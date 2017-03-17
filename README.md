# Niancat.NET

Yet another implementation, in yet another language.

#### Get started

```
./build.ps1 # build the project and run all unit tests
./run.ps1   # start the web server with the default (very limited) wordlist file
./projekt-init.ps1 # bootstrap Projekt, used for handling .fsproj files
```

#### What is this, anyway?

*Nian* (or "*the nine*" in Swedish) is a crossword-ish puzzle in the Swedish
newspaper Svenska Dagbladet. Every day, they publish a grid of nine letters,
and the goal is to find as many words as possible that can be built from those
nine letters. In each problem, at least one word can be built by using all nine
letters.

Example:

    L S A
    O T B
    A S R

The solution? ALBATROSS (incidentally, a valid in both Swedish and English!).

#### Why this?

The project is mainly a learning excercise, to help me improve my skills in a
number of areas:

* F# and functional programming
* Domain-driven design
* CQRS
* Event sourcing
* Property-based testing

The eventual goal is to encapsulate the entire domain with this API, and then
build a separate, small application which acts as a chat bot in e.g. Slack, that
lets people play the game in chat.

#### Credit where credit is due

This project is based on the book
[F# Applied](http://products.tamizhvendan.in/fsharp-applied/) by Tamizhvendan S.,
and my implementation is heavily influenced by samples from the book.
