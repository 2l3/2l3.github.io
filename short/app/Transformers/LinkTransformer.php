<?php

namespace App\Transformers;

use App\Models\Link;
use Flugg\Responder\Transformers\Transformer;

class LinkTransformer extends Transformer
{
    /**
     * List of available relations.
     *
     * @var string[]
     */
    protected $relations = [];

    /**
     * List of autoloaded default relations.
     *
     * @var array
     */
    protected $load = [];

    /**
     * Transform the model.
     *
     * @param  Link  $link
     * @return array
     */
    public function transform(Link $link)
    {
        return [
            'id' => (int) $link->id,
            'code' => (string) $link->code,
            'link' => (string) $link->getLink(),
            'qr' => (string) $link->getQr(),
            'favicon' => (string) $link->getFavicon(),
            'url' => (string) $link->url,
            'type' => (int) $link->type,
            'title' => (string) $link->title,
            'tags' => $link->getTags(),
            'description' => (string) $link->description,
            'has_password' => (bool) $link->password,
            'password' => (string) $link->password,
            'archived' => (bool) $link->archived,
            'disabled' => (bool) $link->disabled,
            'visits' => $link->visits->count(),
            'user' => $link->user_id ? (new UserTransformer())->transform($link->user) : null,
            'domain' => $link->domain_id ? (new DomainTransformer())->transform($link->domain) : null,
            'domain_id' => (int) $link->domain_id ? $link->domain_id : null,
            'expires_at' => $link->expires_at ? $link->expires_at->toISOString() : null,
            'created_at' => $link->created_at->toISOString(),
            'updated_at' => $link->updated_at->toISOString(),
        ];
    }
}
