# Modular System

> ⚠️ **This project is a work in progress.** It is not yet ready for production use.

A modular system implementation in Godot 4.x that allows for dynamic component-based game design.

## Overview

This project provides a flexible and extensible modular system framework for Godot games. It enables developers to create reusable components that can be mixed and matched to create complex game system and behaviors.

## Key features of this modular library:

- Service Locator Pattern: Provides easy access to modules throughout your game
- Module System: Each module is self-contained and follows the IComponent interface
- Event System: Optional event manager for loose coupling between modules

- Easy-to-use API
- Runtime module loading/unloading
- Efficient resource management
- Support for both 2D and 3D games

To extend this library, you can:

- Add new modules by creating a new class that inherits from ModuleBase
- Register new services in the ServiceLocator
- Use the EventManager for communication between modules
- Add new features to existing modules

## Getting Started

Documentation and examples coming soon.

## License

This project is licensed under the MIT License - see the LICENSE file for details.