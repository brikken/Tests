module PrivateType

module TypesRaw =
    type Currency = private Currency of string

    module Currency =
        let create currencies currency : Option<Currency> =
            if List.contains currency currencies
                then Some (Currency currency)
                else None
        let value (Currency currency) = currency

module Types =
    open TypesRaw

    module Currency =
        let private currencies = [ "EUR"; "GBP" ]
        let create = Currency.create currencies
        let value = Currency.value

open Types

let currency1 = Currency.create "GBP"
let currency1a = Currency.create "GBP"
let currency2 = Currency.create "USD"
let currency3 = Option.map Currency.value currency1
let currency4 = Option.map Currency.value currency2
let equal1 = Option.map2 (=) currency1 currency2
let equal2 = Option.map2 (=) currency1 currency1a
