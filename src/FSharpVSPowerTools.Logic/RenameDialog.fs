﻿namespace FSharpVSPowerTools.Refactoring
 
open System
open System.Windows
open System.Windows.Input
open System.ComponentModel

type RenameDialog = FSharpx.XAML<"RenameDialog.xaml">


/// An interface for pre- and post- rename validation
type IRenameValidator =
    abstract ValidateName : string -> Choice<unit, string>

[<RequireQualifiedAccess>]
module UI =
    let loadRenameDialog(viewModel, renameValidator : IRenameValidator) =
        let window = RenameDialog()
        window.btnOk.Click.Add(fun _ -> 
            match renameValidator.ValidateName(window.txtName.Text) with
            | Choice1Of2 _ ->
                window.Root.DialogResult <- Nullable true
                window.Root.Close()
            | Choice2Of2 errorMsg ->
                window.Root.DialogResult <- Nullable false
                MessageBox.Show(errorMsg, "F# Power Tools") |> ignore)
        window.btnCancel.Click.Add(fun _ -> 
            window.Root.DialogResult <- Nullable false
            window.Root.Close())
        window.Root.DataContext <- viewModel
        window.Root
 