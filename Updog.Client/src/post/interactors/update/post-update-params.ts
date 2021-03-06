import { User } from '@/user/domain/user';
import { UserLogin } from '@/user/domain/user-login';

/**
 * Parameters to update a post.
 */
export class PostUpdateParams {
    /**
     * Create a new set of post update parameters.
     * @param postId The ID of the post to update.
     * @param body The new body of the post.
     */
    constructor(public postId: number, public body: string) {}
}
