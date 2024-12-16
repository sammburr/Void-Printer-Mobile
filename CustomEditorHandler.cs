using System;
using Microsoft.Maui.Platform;
using UIKit;


namespace HelloWorld.Platforms.iOS;

public class CustomEditorHandler : Microsoft.Maui.Handlers.EditorHandler
{
    protected override MauiTextView CreatePlatformView()
    {
        var textView = base.CreatePlatformView();

        // Create the custom toolbar
        UIToolbar toolbar = new UIToolbar
        {
            Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 44)
        };

        UIBarButtonItem customButton = new UIBarButtonItem("Submit To The Void", UIBarButtonItemStyle.Done, (sender, e) =>
        {

            textView.ResignFirstResponder(); // Dismiss keyboard

            if(App.Current.MainPage is MainPage page)
                page.OnSubmit();

        });

        UIBarButtonItem cancelButton = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Plain, (ISemanticScreenReader, e) =>
        {
            textView.ResignFirstResponder(); // Dismiss keyboard
        });

        toolbar.Items = new UIBarButtonItem[]
        {
            customButton,
            new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
            cancelButton
        };

        textView.InputAccessoryView = toolbar;

        return textView;
    }
}
