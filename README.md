# Duckie - Multi-function System Tool

[中文](./README.zh-CN.md)

A professional Windows (WPF) application that integrates image processing, PAC proxy management, system volume control, and other system utilities.

---

## ✨ Core Features

### 🖼️ Image Processing
- **Basic Editing**: 90° rotation, horizontal/vertical flip with intuitive controls
- **Zoom & Preview**: 10%-300% zoom control, mouse wheel zoom, and fit-to-window
- **Drag & Drop**: Smart file loading with support for PNG, JPG, BMP, GIF formats  
- **Export Functions**: Image export and ICO icon generation

### 🌐 PAC Proxy Management
- **Configuration Management**: Add, edit, delete PAC configurations with quick switching
- **Status Indicators**: Clear visual status with system tray icon badges
- **Registry Integration**: Automatic Windows system proxy settings management
- **Direct Connection**: One-click proxy disable with system tray quick access

### 🔊 System Volume Control
- **Global Hotkeys**: Alt+Shift+8/9/0 for volume down/up/mute toggle
- **Floating Overlay**: Modern volume overlay with auto-fade animation
- **Smart Icons**: Volume status-based icon switching

### 🛠️ System Utilities
- **App Display & Hide**: System tray-based window show/hide
- **App Exit**: Clean application termination
- **Volume Adjustment**: Fine-grained volume control
- **Volume Mute**: Quick mute/unmute functionality
- **Quiet Mode**: Background operation mode
- **Icon Conversion**: Convert images to ICO format
- **Online Updates**: Automatic update checking (Desktop only)
- **Code Generation**: Development utility features

### ⚙️ System Integration
- **System Tray**: Background operation with left-click show/hide
- **Startup Options**: Optional system startup integration
- **Hotkey Management**: Customizable global hotkey system
- **Modern Interface**: Clean WPF UI with collapsible sidebar

## 🚀 Quick Start

### Image Processing
1. Drag images to window or click "Open" to load images
2. Use toolbar buttons for rotation and flip operations  
3. Mouse wheel + Ctrl for zoom, or use zoom controls
4. Click "Save" or "Export ICO" to save results

### PAC Proxy Management  
1. Click "Add PAC" to add proxy configuration (Name + PAC URL)
2. Click configuration cards to apply proxy, click "No PAC" to disable
3. Use "Edit"/"Delete" buttons to manage existing configurations
4. System tray right-click menu for quick proxy switching

### Volume Control
- **Alt + Shift + 8**: Volume down
- **Alt + Shift + 9**: Volume up
- **Alt + Shift + 0**: Mute/unmute toggle

## 📋 Platform Compatibility

| Feature | UI Available | Right-Click Menu | Quick Keys | State Display | MS Store Available |
|---------|-------------|-----------------|------------|---------------|-------------------|
| App Show/Hide | ✗ | ✓ | ✓ | ✗ | ✓ |
| App Exit | ✗ | ✓ | ✓ | ✗ | ✓ |
| Volume Down | ✗ | ✓ | ✗ | ✗ | ✓ |
| Volume Up | ✗ | ✓ | ✗ | ✗ | ✓ |
| Quiet Mode/Mute | ✗ | ✓ | ✗ | ✗ | ✓ |
| Icon Conversion | ✓ | ✗ | ✗ | ✗ | ✓ |
| PAC Switch | ✓ | ✓ | ✓ | ✓ | ✗ |
| Online Updates | ✗ | ✓ | ✗ | ✗ | ✗ |
| Code Generation | ✓ | ✗ | ✗ | ✗ | ✗ |

## 💡 Technical Highlights

- **Memory Optimization**: Smart resource management with timely memory release after image processing
- **Asynchronous Operations**: Non-blocking UI with smooth responsiveness  
- **Error Handling**: Comprehensive exception handling with user-friendly error messages
- **Modular Architecture**: Clear service layering for easy feature extension
- **Cross-Platform Ready**: Designed for both desktop and Microsoft Store deployment

