// Type definitions for bootstrap-dialog
// Project: https://github.com/nakupanda/bootstrap3-dialog
// Definitions by: pwelter34
// Definitions: https://github.com/borisyankov/DefinitelyTyped

interface BootstrapDialogOptions {
    type?: string;
    size?: string;
    cssClass?: string;
    title?: any;
    message?: any;
    buttons?: Array<any>;
    closable?: boolean;
    spinicon?: string;
    data?: any;

    onshow?: (dialog?: BootstrapDialog) => void;
    onshown?: (dialog?: BootstrapDialog) => void;
    onhide?: (dialog?: BootstrapDialog) => void;
    onhidden?: (dialog?: BootstrapDialog) => void;

    autodestroy?: boolean;
    draggable?: boolean;
    animate?: boolean;
    description?: string;
}

interface BootstrapDialog {
    open(): BootstrapDialog;
    close(): BootstrapDialog;

    getModal(): any;
    getModalDialog(): any;
    getModalContent(): any;
    getModalHeader(): any;
    getModalBody(): any;
    getModalFooter(): any;

    getData(key: string): any;
    setData(key: string, value: any): BootstrapDialog;

    enableButtons(enable: boolean): BootstrapDialog;
    setClosable(closable: boolean): BootstrapDialog;

    realize(): BootstrapDialog;
}

interface BootstrapDialogStatic {
    configDefaultOptions(options: BootstrapDialogOptions): void;

    openAll(): void;
    closeAll(): void;
    moveFocus(): void;
    showScrollbar(): void;

    show(options: BootstrapDialogOptions): BootstrapDialog;
    alert(message: string, callback?: (result: boolean) => void): BootstrapDialog;
    confirm(message: string, callback?: (result: boolean) => void): BootstrapDialog;
    warning(message: string, callback?: (result: boolean) => void): BootstrapDialog;
    danger(message: string, callback?: (result: boolean) => void): BootstrapDialog;
    success(message: string, callback?: (result: boolean) => void): BootstrapDialog;
}

declare var BootstrapDialog: BootstrapDialogStatic;