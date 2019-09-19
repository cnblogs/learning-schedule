import { RESPONSE } from '@nguniversal/express-engine/tokens';
import { Inject, Injectable, Optional } from '@angular/core';

@Injectable()

export class ServerResponseService {
    constructor(
        @Optional() @Inject(RESPONSE) private response: any
    ) {
    }

    public setNotFound(message: string = 'not found') {
        if (this.response) {
            this.response.statusCode = 404;
            this.response.statusMessage = message;
        }
    }

    getHeader(key: string): string {
        return this.response.getHeader(key) as string;
    }

    setHeader(key: string, value: string): void {
        if (this.response) {
            this.response.header(key, value);
        }
    }

    appendHeader(key: string, value: string, delimiter = ','): void {
        if (this.response) {
            const current = this.getHeader(key);
            if (!current) {
                return this.setHeader(key, value);
            }

            const newValue = [...current.split(delimiter), value]
                .filter((el, i, a) => i === a.indexOf(el))
                .join(delimiter);

            this.response.header(key, newValue);
        }
    }

    setStatus(code: number, message?: string): void {
        if (this.response) {
            this.response.statusCode = code;
            if (message) {
                this.response.statusMessage = message;
            }
        }
    }
}