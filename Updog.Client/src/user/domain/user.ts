/**
 * A user of the website.
 */
export class User {
    /**
     * Create a new user.
     * @param id The unique ID of the user.
     * @param username Their display name.
     * @param joinedDate When they registered.
     * @param email The contact email (if any).
     * @param postKarma The number of post upvotes.
     * @param commentKarma The number of comment upvotes.
     */
    constructor(
        public id: number,
        public username: string,
        public joinedDate: Date,
        public postKarma: number,
        public commentKarma: number
    ) {}
}
