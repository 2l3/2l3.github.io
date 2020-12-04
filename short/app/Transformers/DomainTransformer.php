<?php

namespace App\Transformers;

use App\Models\Domain;
use Flugg\Responder\Transformers\Transformer;

class DomainTransformer extends Transformer
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
     * @param  Domain  $domain
     * @return array
     */
    public function transform(Domain $domain)
    {
        return [
            'id' => (int) $domain->id,
            'domain' => (string) $domain->domain,
            'redirect' => (string) $domain->redirect,
            'status' => (bool) $domain->status,
            'created_at' => (string) $domain->created_at->toISOString(),
        ];
    }
}
