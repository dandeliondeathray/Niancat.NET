module Niancat.Persistence.InMemory.EventStore

open Problem
open Wordlist

open Niancat.Persistence.Queries
open Niancat.Persistence.Projections
open Niancat.Persistence.EventStore

open Niancat.Core.Events
open NEventStore

type InMemoryEventStore () =
    static member Instance =
        Wireup.Init()
            .UsingInMemoryPersistence()
            .Build()

let inMemoryEventStore () =
    let eventStoreInstance = InMemoryEventStore.Instance
    {
        getState = getState eventStoreInstance
        saveEvents = saveEvents eventStoreInstance
    }

let inMemoryQueries : Queries = {
    problem = problemQueries
    wordlist = wordlistQueries
}

let inMemoryActions : ProjectionActions = {
    problem = problemActions
    wordlist = wordlistActions
}