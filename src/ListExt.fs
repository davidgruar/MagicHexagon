module List

/// <summary>
/// headsAndTails ["a"; "b"; "c"] returns [("a", ["b"; "c"]); ("b", ["a"; "c"]); ("c", ["b"; "a"])]
/// </summary>
let headsAndTails list =
    let rec inner acc list' =
        match list' with
        | [] -> []
        | hd::tl -> (hd, List.append acc tl)::(inner (hd::acc) tl)
    inner [] list