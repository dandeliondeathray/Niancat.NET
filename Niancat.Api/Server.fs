module Niancat.Api.Server

open Suave
open Niancat.Application.ApplicationServices

let startServer applicationService cancellationToken =
     startWebServerAsync
            { defaultConfig with cancellationToken = cancellationToken }
            (WebParts.niancat applicationService)
        |> snd