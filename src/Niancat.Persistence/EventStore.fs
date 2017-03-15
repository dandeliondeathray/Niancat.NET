module Niancat.Persistence.EventStore

open Niancat.Core.Events
open Niancat.Core.States

open NEventStore
open System

let private eventStoreId = "niancat"

let saveEvent (store : IStoreEvents) event =
    use stream = store.OpenStream(eventStoreId)
    stream.Add(EventMessage(Body = event))
    stream.CommitChanges(Guid.NewGuid())

let saveEvents store =
    List.iter (saveEvent store)
    >> async.Return

let getEvents (store : IStoreEvents) =
    use stream = store.OpenStream(eventStoreId)
    stream.CommittedEvents
    |> Seq.map (fun msg -> msg.Body)
    |> Seq.cast<Event>

let getStateFromEvents<'events> = Seq.fold apply TabulaRasa

let getState store () = getEvents store |> getStateFromEvents |> async.Return

type EventStore = {
    getState : unit -> Async<ApplicationState>
    saveEvents : Event list -> Async<unit>
}
