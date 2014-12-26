interface ObjectIdStatic {
    new (): IObjectId;
    new (value: string): IObjectId;
    new (timestamp: number, machine: number, pid: number, increment: number): IObjectId;
}

interface IObjectId {
    timestamp: number;
    machine: number;
    pid: number;
    increment: number;

    getDate(): Date;
    toArray(): number[];
    toString(): string;
}

declare var ObjectId: ObjectIdStatic;
