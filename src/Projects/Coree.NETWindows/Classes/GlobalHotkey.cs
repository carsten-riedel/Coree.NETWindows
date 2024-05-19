using Coree.NETWindows.NativeMethods;

namespace Coree.NETWindows.Classes
{
    /// <summary>
    /// Manages a global hotkey registration that allows for handling key events system-wide.
    /// </summary>
    public class GlobalHotkey : IDisposable
    {
        private static int nextId = 1; // Static counter for IDs

        private int id;
        private IntPtr handle;
        private Keys key;
        private KeyModifier modifier;
        private Action? _action;
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the GlobalHotkey class with specified key and modifier using integer values, allowing actions to be attached.
        /// <code>
        ///     private GlobalHotkey? HelloWorldHotKey;
        ///
        ///     public Form1()
        ///     {
        ///         InitializeComponent();
        ///         this.CreateHandle();
        ///         HelloWorldHotKey = new GlobalHotkey(this.Handle, Coree.NETWindows.Keys.H, Coree.NETWindows.KeyModifier.Alt, () =>
        ///         {
        ///              MessageBox.Show("Hello World !");
        ///         });
        ///     }
        ///     protected override void WndProc(ref Message m)
        ///     {
        ///         base.WndProc(ref m);
        ///         HelloWorldHotKey?.Match(m.Msg, m.WParam);
        ///     }
        /// </code>
        /// </summary>
        /// <param name="handle">The handle to the window that will process the hotkey event.</param>
        /// <param name="key">The key code represented as an integer.</param>
        /// <param name="modifier">The modifier code represented as an integer.</param>
        /// <param name="action">The action to execute when the hotkey is pressed.</param>
        /// <param name="autoRegister">Whether to automatically register the hotkey after initialization.</param>
        public GlobalHotkey(IntPtr handle, int key, int modifier, Action? action, bool autoRegister = true)
        {
            Init(handle, (Coree.NETWindows.Keys)key, (Coree.NETWindows.KeyModifier)modifier, action, autoRegister);
        }

        /// <summary>
        /// Initializes a new instance of the GlobalHotkey class with specified key and modifier, allowing actions to be attached.
        /// <code>
        ///     private GlobalHotkey? HelloWorldHotKey;
        ///
        ///     public Form1()
        ///     {
        ///         InitializeComponent();
        ///         this.CreateHandle();
        ///         HelloWorldHotKey = new GlobalHotkey(this.Handle, Coree.NETWindows.Keys.H, Coree.NETWindows.KeyModifier.Alt, () =>
        ///         {
        ///              MessageBox.Show("Hello World !");
        ///         });
        ///     }
        ///     protected override void WndProc(ref Message m)
        ///     {
        ///         base.WndProc(ref m);
        ///         HelloWorldHotKey?.Match(m.Msg, m.WParam);
        ///     }
        /// </code>
        /// </summary>
        /// <param name="handle">The handle to the window that will process the hotkey event.</param>
        /// <param name="key">The key code.</param>
        /// <param name="modifier">The modifier.</param>
        /// <param name="action">The action to execute when the hotkey is pressed.</param>
        /// <param name="autoRegister">Whether to automatically register the hotkey after initialization.</param>
        public GlobalHotkey(IntPtr handle, Keys key, KeyModifier modifier, Action? action, bool autoRegister = true)
        {
            Init(handle, key, modifier, action, autoRegister);
        }

        /// <summary>
        /// Initializes the hotkey with specified parameters and registers it if specified.
        /// </summary>
        /// <param name="handle">The window handle associated with the hotkey.</param>
        /// <param name="key">The key associated with the hotkey.</param>
        /// <param name="modifier">The modifiers used with the key.</param>
        /// <param name="action">The action to invoke when the hotkey is triggered.</param>
        /// <param name="autoRegister">Specifies whether to automatically register the hotkey.</param>
        private void Init(IntPtr handle, Keys key, KeyModifier modifier, Action? action, bool autoRegister = true)
        {
            this.key = key;
            this.modifier = modifier;
            this.handle = handle;
            this.id = nextId++; // Assign and increment the static counter
            this._action = action;
            if (autoRegister)
            {
                this.Register();
            }
        }

        /// <summary>
        /// Gets the identifier for this hotkey instance.
        /// </summary>
        public int Id => this.id;

        /// <summary>
        /// Registers the hotkey with the operating system.
        /// </summary>
        /// <returns>True if registration is successful; otherwise false.</returns>
        public bool Register()
        {
            return KeyboardManagement.RegisterHotKey(handle, id, (uint)modifier, (uint)key);
        }

        /// <summary>
        /// Unregisters the hotkey from the operating system.
        /// </summary>
        /// <returns>True if unregistration is successful; otherwise false.</returns>
        private bool Unregister()
        {
            return KeyboardManagement.UnregisterHotKey(handle, id);
        }

        /// <summary>
        /// Checks if the incoming system message corresponds to the hotkey event and invokes the action if it does.
        /// </summary>
        /// <param name="msg">The message identifier.</param>
        /// <param name="wparam">The message's WPARAM value, which should match the hotkey ID.</param>
        public void Match(int msg, IntPtr wparam)
        {
            if (msg == KeyboardManagement.WM_HOTKEY && wparam.ToInt32() == id)
            {
                _action?.Invoke();
            }
        }

        /// <summary>
        /// Disposes of the hotkey by unregistering it and marking the instance as disposed.
        /// </summary>
        /// <param name="disposing">Indicates whether the method call comes from a Dispose method (its value is true) or from a finalizer (its value is false).</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Unregister();
                disposedValue = true;
            }
        }

        /// <summary>
        /// Finalizer that ensures the hotkey is unregistered when the object is collected by the garbage collector.
        /// </summary>
        ~GlobalHotkey()
        {
            Dispose(disposing: false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}